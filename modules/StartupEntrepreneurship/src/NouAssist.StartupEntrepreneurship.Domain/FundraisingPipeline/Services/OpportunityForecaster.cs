using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Aggregates;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Enums;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Exceptions;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Repositories;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Services.Interfaces;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Services
{
    /// <summary>
    /// Domain service for forecasting investment opportunities
    /// </summary>
    public class OpportunityForecaster : DomainService, IOpportunityForecaster
    {
        private readonly IProspectRepository _prospectRepository;
        private readonly ILogger<OpportunityForecaster> _logger;
        
        public OpportunityForecaster(
            IProspectRepository prospectRepository,
            ILogger<OpportunityForecaster> logger)
        {
            _prospectRepository = prospectRepository;
            _logger = logger;
        }
        
        /// <summary>
        /// Generates an opportunity forecast for a prospect
        /// </summary>
        public async Task<OpportunityForecastValue> GenerateForecastAsync(
            Guid prospectId,
            decimal baseAmount)
        {
            var prospect = await _prospectRepository.GetAsync(prospectId);
            if (prospect == null)
            {
                throw new ProspectNotFoundException(prospectId);
            }
            
            try
            {
                // Calculate probability based on status and score
                double probability = CalculateProbability(prospect);
                
                // Calculate forecast values
                decimal minAmount = CalculateMinAmount(baseAmount, prospect);
                decimal maxAmount = CalculateMaxAmount(baseAmount, prospect);
                decimal expectedAmount = CalculateExpectedAmount(minAmount, maxAmount, probability);
                
                // Estimate closing date
                DateTime forecastedClosingDate = EstimateClosingDate(prospect);
                
                var forecast = new OpportunityForecastValue(
                    minAmount,
                    maxAmount,
                    expectedAmount,
                    probability,
                    forecastedClosingDate);
                
                _logger.LogInformation(
                    "Generated forecast for prospect {ProspectId}: Expected={Expected}, Probability={Probability}",
                    prospectId, expectedAmount, probability);
                
                return forecast;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate forecast for prospect {ProspectId}", prospectId);
                throw;
            }
        }
        
        /// <summary>
        /// Updates the opportunity forecast for a prospect
        /// </summary>
        public async Task UpdateForecastAsync(
            Guid prospectId,
            decimal baseAmount)
        {
            var prospect = await _prospectRepository.GetAsync(prospectId);
            if (prospect == null)
            {
                throw new ProspectNotFoundException(prospectId);
            }
            
            try
            {
                var forecast = await GenerateForecastAsync(prospectId, baseAmount);
                
                prospect.UpdateOpportunityForecast(
                    forecast.MinAmount,
                    forecast.MaxAmount,
                    forecast.ExpectedAmount,
                    forecast.Probability,
                    forecast.ForecastedClosingDate);
                
                await _prospectRepository.UpdateAsync(prospect);
                
                _logger.LogInformation(
                    "Updated forecast for prospect {ProspectId}",
                    prospectId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update forecast for prospect {ProspectId}", prospectId);
                throw;
            }
        }
        
        /// <summary>
        /// Calculates the probability of closing a deal with the prospect
        /// </summary>
        private double CalculateProbability(Prospect prospect)
        {
            // Base probability from pipeline status
            double baseProbability;
            switch (prospect.PipelineStatus.Status)
            {
                case ProspectStatus.Committed:
                    baseProbability = 0.95;
                    break;
                case ProspectStatus.Negotiating:
                    baseProbability = 0.8;
                    break;
                case ProspectStatus.TermSheetReceived:
                    baseProbability = 0.7;
                    break;
                case ProspectStatus.InDueDiligence:
                    baseProbability = 0.5;
                    break;
                case ProspectStatus.Pitched:
                    baseProbability = 0.3;
                    break;
                case ProspectStatus.Engaged:
                    baseProbability = 0.2;
                    break;
                case ProspectStatus.Contacted:
                    baseProbability = 0.1;
                    break;
                default:
                    baseProbability = 0.05;
                    break;
            }
            
            // Adjust by score (overall score impacts by up to +/- 20%)
            double scoreAdjustment = (prospect.Score.OverallScore - 50) / 250.0; // Range: -0.2 to +0.2
            
            // Adjust by interaction recency
            double recencyAdjustment = 0;
            if (prospect.Interactions.Any())
            {
                var mostRecent = prospect.Interactions.Max(i => i.InteractionDate);
                var daysSince = (DateTime.Now - mostRecent).TotalDays;
                
                if (daysSince <= 7)
                    recencyAdjustment = 0.05;
                else if (daysSince >= 60)
                    recencyAdjustment = -0.1;
            }
            
            // Combine and ensure within bounds
            double probability = baseProbability + scoreAdjustment + recencyAdjustment;
            return Math.Clamp(probability, 0.01, 0.99);
        }
        
        private decimal CalculateMinAmount(decimal baseAmount, Prospect prospect)
        {
            // Conservative estimate - varies based on status
            switch (prospect.PipelineStatus.Status)
            {
                case ProspectStatus.Committed:
                case ProspectStatus.Negotiating:
                case ProspectStatus.TermSheetReceived:
                    return baseAmount * 0.9m; // 90% of base for late stages
                default:
                    return baseAmount * 0.5m; // 50% of base for early stages
            }
        }
        
        private decimal CalculateMaxAmount(decimal baseAmount, Prospect prospect)
        {
            // Optimistic estimate - varies based on status
            switch (prospect.PipelineStatus.Status)
            {
                case ProspectStatus.Committed:
                    return baseAmount * 1.1m; // 110% of base for committed
                case ProspectStatus.Negotiating:
                case ProspectStatus.TermSheetReceived:
                    return baseAmount * 1.5m; // 150% of base for term sheet/negotiation
                default:
                    return baseAmount * 2.0m; // 200% of base for early stages
            }
        }
        
        private decimal CalculateExpectedAmount(decimal min, decimal max, double probability)
        {
            // Weighted average, skewed by probability
            return min + (max - min) * (decimal)probability;
        }
        
        private DateTime EstimateClosingDate(Prospect prospect)
        {
            // Base closing time depends on current status
            int daysToClose;
            switch (prospect.PipelineStatus.Status)
            {
                case ProspectStatus.Committed:
                    daysToClose = 14; // 2 weeks
                    break;
                case ProspectStatus.Negotiating:
                    daysToClose = 30; // 1 month
                    break;
                case ProspectStatus.TermSheetReceived:
                    daysToClose = 45; // 1.5 months
                    break;
                case ProspectStatus.InDueDiligence:
                    daysToClose = 60; // 2 months
                    break;
                case ProspectStatus.Pitched:
                    daysToClose = 90; // 3 months
                    break;
                case ProspectStatus.Engaged:
                    daysToClose = 120; // 4 months
                    break;
                default:
                    daysToClose = 180; // 6 months
                    break;
            }
            
            return DateTime.Now.AddDays(daysToClose);
        }
    }
}
