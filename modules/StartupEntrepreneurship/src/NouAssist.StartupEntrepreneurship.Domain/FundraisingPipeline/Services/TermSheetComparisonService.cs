using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Aggregates;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Enums;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Repositories;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Services.Interfaces;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Services
{
    /// <summary>
    /// Domain service for comparing term sheets
    /// </summary>
    public class TermSheetComparisonService : DomainService, ITermSheetComparisonService
    {
        private readonly ITermSheetOfferRepository _termSheetRepository;
        private readonly ILogger<TermSheetComparisonService> _logger;
        
        public TermSheetComparisonService(
            ITermSheetOfferRepository termSheetRepository,
            ILogger<TermSheetComparisonService> logger)
        {
            _termSheetRepository = termSheetRepository;
            _logger = logger;
        }
        
        /// <summary>
        /// Compares multiple term sheets
        /// </summary>
        public async Task<TermSheetComparisonResult> CompareTermSheetsAsync(List<Guid> termSheetIds)
        {
            if (termSheetIds.Count < 2)
            {
                throw new ArgumentException("At least two term sheets are required for comparison.");
            }
            
            try
            {
                var termSheets = new List<TermSheetOffer>();
                
                foreach (var id in termSheetIds)
                {
                    var termSheet = await _termSheetRepository.GetWithDetailsAsync(id);
                    if (termSheet == null)
                    {
                        throw new BusinessException("FundraisingPipeline:ErrorMessage:TermSheetNotFound")
                            .WithData("id", id);
                    }
                    
                    termSheets.Add(termSheet);
                }
                
                var result = new TermSheetComparisonResult
                {
                    ComparisonDate = DateTime.Now,
                    TermSheetDetails = new Dictionary<Guid, TermSheetSummary>(),
                    ValuationComparison = new Dictionary<Guid, decimal>(),
                    EquityPercentageComparison = new Dictionary<Guid, decimal>(),
                    LiquidationMultipleComparison = new Dictionary<Guid, decimal>(),
                    KeyClauseComparison = new Dictionary<string, Dictionary<Guid, string>>(),
                    InvestorRightsComparison = new Dictionary<string, Dictionary<Guid, bool>>()
                };
                
                // Populate term sheet details and basic comparisons
                foreach (var termSheet in termSheets)
                {
                    result.TermSheetDetails[termSheet.Id] = new TermSheetSummary
                    {
                        Id = termSheet.Id,
                        Title = termSheet.Title,
                        ProspectId = termSheet.ProspectId,
                        ReceivedDate = termSheet.ReceivedDate,
                        Status = termSheet.Status
                    };
                    
                    result.ValuationComparison[termSheet.Id] = termSheet.TermData.PreMoneyValuation;
                    result.EquityPercentageComparison[termSheet.Id] = termSheet.TermData.EquityPercentage;
                    result.LiquidationMultipleComparison[termSheet.Id] = termSheet.TermData.LiquidationMultiple;
                }
                
                // Compare clauses across term sheets
                var allClauseCategories = new HashSet<string>();
                foreach (var termSheet in termSheets)
                {
                    foreach (var clause in termSheet.Clauses)
                    {
                        allClauseCategories.Add(clause.Category);
                    }
                }
                
                foreach (var category in allClauseCategories)
                {
                    var categoryComparison = new Dictionary<Guid, string>();
                    
                    foreach (var termSheet in termSheets)
                    {
                        var clause = termSheet.Clauses.FirstOrDefault(c => c.Category == category);
                        categoryComparison[termSheet.Id] = clause?.Text.Content ?? "Not included";
                    }
                    
                    result.KeyClauseComparison[category] = categoryComparison;
                }
                
                // Compare investor rights
                var allRights = new HashSet<string>();
                foreach (var termSheet in termSheets)
                {
                    foreach (var right in termSheet.TermData.InvestorRights.Keys)
                    {
                        allRights.Add(right);
                    }
                }
                
                foreach (var right in allRights)
                {
                    var rightComparison = new Dictionary<Guid, bool>();
                    
                    foreach (var termSheet in termSheets)
                    {
                        rightComparison[termSheet.Id] = termSheet.TermData.InvestorRights.GetValueOrDefault(right, false);
                    }
                    
                    result.InvestorRightsComparison[right] = rightComparison;
                }
                
                _logger.LogInformation(
                    "Compared {Count} term sheets",
                    termSheets.Count);
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to compare term sheets");
                throw;
            }
        }
        
        /// <summary>
        /// Compares two term sheets and identifies the better option based on founder-friendly criteria
        /// </summary>
        public async Task<TermSheetRankingResult> RankTermSheetsAsync(List<Guid> termSheetIds)
        {
            var comparison = await CompareTermSheetsAsync(termSheetIds);
            
            try
            {
                var result = new TermSheetRankingResult
                {
                    ComparisonResult = comparison,
                    RankingDate = DateTime.Now,
                    TermSheetScores = new Dictionary<Guid, int>(),
                    RankingExplanations = new Dictionary<Guid, List<string>>()
                };
                
                // Score and rank each term sheet
                foreach (var termSheetId in termSheetIds)
                {
                    int score = 0;
                    var explanations = new List<string>();
                    
                    // Valuation (higher is better)
                    var valuation = comparison.ValuationComparison[termSheetId];
                    var maxValuation = comparison.ValuationComparison.Values.Max();
                    var valuationScore = (int)((valuation / maxValuation) * 30); // Up to 30 points
                    score += valuationScore;
                    explanations.Add($"Valuation: {valuation:C0} ({valuationScore} points)");
                    
                    // Equity percentage (lower is better)
                    var equity = comparison.EquityPercentageComparison[termSheetId];
                    var minEquity = comparison.EquityPercentageComparison.Values.Min();
                    var equityScore = (int)((minEquity / equity) * 30); // Up to 30 points
                    score += equityScore;
                    explanations.Add($"Equity percentage: {equity:F2}% ({equityScore} points)");
                    
                    // Liquidation multiple (lower is better)
                    var liquidationMultiple = comparison.LiquidationMultipleComparison[termSheetId];
                    var liquidationScore = liquidationMultiple == 1 ? 20 : (liquidationMultiple <= 1.5m ? 10 : 0);
                    score += liquidationScore;
                    explanations.Add($"Liquidation multiple: {liquidationMultiple}x ({liquidationScore} points)");
                    
                    // Investor rights (fewer is better, simplified scoring)
                    int rightsScore = 0;
                    int totalRights = comparison.InvestorRightsComparison.Count;
                    int rightsPenalty = 0;
                    
                    foreach (var right in comparison.InvestorRightsComparison)
                    {
                        if (right.Value[termSheetId])
                        {
                            rightsPenalty += 1;
                        }
                    }
                    
                    if (totalRights > 0)
                    {
                        rightsScore = 20 - (int)((double)rightsPenalty / totalRights * 20);
                    }
                    else
                    {
                        rightsScore = 20;
                    }
                    
                    score += rightsScore;
                    explanations.Add($"Investor rights: {rightsPenalty} rights requested ({rightsScore} points)");
                    
                    result.TermSheetScores[termSheetId] = score;
                    result.RankingExplanations[termSheetId] = explanations;
                }
                
                _logger.LogInformation(
                    "Ranked {Count} term sheets",
                    termSheetIds.Count);
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to rank term sheets");
                throw;
            }
        }
    }
    
    /// <summary>
    /// Container for term sheet comparison results
    /// </summary>
    public class TermSheetComparisonResult
    {
        public DateTime ComparisonDate { get; set; }
        public Dictionary<Guid, TermSheetSummary> TermSheetDetails { get; set; }
        public Dictionary<Guid, decimal> ValuationComparison { get; set; }
        public Dictionary<Guid, decimal> EquityPercentageComparison { get; set; }
        public Dictionary<Guid, decimal> LiquidationMultipleComparison { get; set; }
        public Dictionary<string, Dictionary<Guid, string>> KeyClauseComparison { get; set; }
        public Dictionary<string, Dictionary<Guid, bool>> InvestorRightsComparison { get; set; }
    }
    
    /// <summary>
    /// Container for term sheet ranking results
    /// </summary>
    public class TermSheetRankingResult
    {
        public TermSheetComparisonResult ComparisonResult { get; set; }
        public DateTime RankingDate { get; set; }
        public Dictionary<Guid, int> TermSheetScores { get; set; }
        public Dictionary<Guid, List<string>> RankingExplanations { get; set; }
        
        public List<Guid> GetRankedTermSheets()
        {
            return TermSheetScores
                .OrderByDescending(s => s.Value)
                .Select(s => s.Key)
                .ToList();
        }
        
        public Guid GetTopTermSheet()
        {
            return TermSheetScores
                .OrderByDescending(s => s.Value)
                .First()
                .Key;
        }
    }
    
    /// <summary>
    /// Summary information about a term sheet for comparison
    /// </summary>
    public class TermSheetSummary
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid ProspectId { get; set; }
        public DateTime ReceivedDate { get; set; }
        public TermSheetStatus Status { get; set; }
    }
}
