using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Entities;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Enums;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Interfaces.Services;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Repositories;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Services
{
    /// <summary>
    /// Domain service for SWOT analysis
    /// </summary>
    public class SwotAnalysisService : DomainService, ISwotAnalysisService
    {
        private readonly ISwotAnalysisRepository _swotRepository;
        private readonly ILogger<SwotAnalysisService> _logger;

        public SwotAnalysisService(
            ISwotAnalysisRepository swotRepository,
            ILogger<SwotAnalysisService> logger)
        {
            _swotRepository = swotRepository;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<SwotAnalysis> CreateSwotAnalysisAsync(
            string title,
            string description)
        {
            try
            {
                var analysis = SwotAnalysis.Create(
                    GuidGenerator.Create(),
                    title,
                    description,
                    Clock.Now,
                    CurrentTenant.Id
                );

                await _swotRepository.InsertAsync(analysis);
                return analysis;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create SWOT analysis with title {Title}", title);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<SwotAnalysis> UpdateSwotAnalysisAsync(
            Guid analysisId,
            string title,
            string description,
            string analysisNotes = null)
        {
            try
            {
                var analysis = await _swotRepository.GetAsync(analysisId);
                analysis.UpdateAnalysis(title, description, analysisNotes);
                await _swotRepository.UpdateAsync(analysis);
                return analysis;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update SWOT analysis {AnalysisId}", analysisId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task AddSwotItemAsync(
            Guid analysisId,
            SwotCategory category,
            string title,
            string description,
            decimal impact = 0.5m,
            int priority = 1,
            string actionItems = null,
            string tags = null)
        {
            try
            {
                var analysis = await _swotRepository.GetAsync(analysisId);
                
                if (analysis.IsCompleted)
                {
                    throw new BusinessException("MarketIntelligence:ErrorMessage:CannotModifyCompletedAnalysis");
                }

                analysis.AddItem(
                    category,
                    title,
                    description,
                    impact,
                    priority,
                    actionItems,
                    tags
                );

                await _swotRepository.UpdateAsync(analysis);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add SWOT item to analysis {AnalysisId}", analysisId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task CompleteSwotAnalysisAsync(
            Guid analysisId,
            string strategicRecommendation)
        {
            try
            {
                var analysis = await _swotRepository.GetAsync(analysisId);
                analysis.CompleteAnalysis(strategicRecommendation);
                await _swotRepository.UpdateAsync(analysis);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to complete SWOT analysis {AnalysisId}", analysisId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<SwotAnalysis>> GetRecentSwotAnalysesAsync(int maxResultCount = 10)
        {
            try
            {
                return await _swotRepository.GetRecentAsync(maxResultCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get recent SWOT analyses");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<SwotAnalysis>> GetCompletedSwotAnalysesAsync()
        {
            try
            {
                return await _swotRepository.GetCompletedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get completed SWOT analyses");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<SwotAnalysisQualityResult> EvaluateSwotAnalysisQualityAsync(Guid analysisId)
        {
            try
            {
                var analysis = await _swotRepository.GetAsync(analysisId);
                
                var result = new SwotAnalysisQualityResult
                {
                    AnalysisId = analysisId,
                    Title = analysis.Title,
                    BalanceScore = analysis.GetAnalysisBalanceScore(),
                    CategoryCoverage = CalculateCategoryCoverage(analysis),
                    ItemCount = analysis.Items.Count,
                    IsComplete = analysis.IsCompleted,
                    MissingCategories = GetMissingCategories(analysis)
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to evaluate SWOT analysis quality for {AnalysisId}", analysisId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<Dictionary<SwotCategory, List<SwotItem>>> GetItemsByCategoryAsync(Guid analysisId)
        {
            try
            {
                var analysis = await _swotRepository.GetAsync(analysisId);
                var result = new Dictionary<SwotCategory, List<SwotItem>>();

                foreach (var category in Enum.GetValues(typeof(SwotCategory)).Cast<SwotCategory>())
                {
                    result[category] = analysis.GetItemsByCategory(category).ToList();
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get SWOT items by category for analysis {AnalysisId}", analysisId);
                throw;
            }
        }

        /// <summary>
        /// Calculates category coverage percentages
        /// </summary>
        private Dictionary<SwotCategory, decimal> CalculateCategoryCoverage(SwotAnalysis analysis)
        {
            var result = new Dictionary<SwotCategory, decimal>();
            var categorySummary = analysis.GetItemCategorySummary();
            var totalItems = analysis.Items.Count;

            if (totalItems == 0)
            {
                foreach (var category in Enum.GetValues(typeof(SwotCategory)).Cast<SwotCategory>())
                {
                    result[category] = 0;
                }
                return result;
            }

            foreach (var category in Enum.GetValues(typeof(SwotCategory)).Cast<SwotCategory>())
            {
                var categoryCount = categorySummary.ContainsKey(category) ? categorySummary[category] : 0;
                result[category] = (decimal)categoryCount / totalItems;
            }

            return result;
        }

        /// <summary>
        /// Gets missing SWOT categories
        /// </summary>
        private List<SwotCategory> GetMissingCategories(SwotAnalysis analysis)
        {
            var missingCategories = new List<SwotCategory>();
            var categorySummary = analysis.GetItemCategorySummary();

            foreach (var category in Enum.GetValues(typeof(SwotCategory)).Cast<SwotCategory>())
            {
                if (!categorySummary.ContainsKey(category) || categorySummary[category] == 0)
                {
                    missingCategories.Add(category);
                }
            }

            return missingCategories;
        }
    }

    /// <summary>
    /// SWOT analysis quality evaluation result
    /// </summary>
    public class SwotAnalysisQualityResult
    {
        public Guid AnalysisId { get; set; }
        public string Title { get; set; }
        public decimal BalanceScore { get; set; }
        public Dictionary<SwotCategory, decimal> CategoryCoverage { get; set; }
        public int ItemCount { get; set; }
        public bool IsComplete { get; set; }
        public List<SwotCategory> MissingCategories { get; set; }
    }
}
