using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Aggregates;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Entities;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Repositories;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Services.Interfaces;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Specifications;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Services
{
    /// <summary>
    /// Domain service for strategic pivot logging and tracking
    /// </summary>
    public class PivotLogger : DomainService, IPivotLogger
    {
        private readonly IStrategicPivotRepository _pivotRepository;
        private readonly ILogger<PivotLogger> _logger;

        public PivotLogger(
            IStrategicPivotRepository pivotRepository,
            ILogger<PivotLogger> logger)
        {
            _pivotRepository = pivotRepository;
            _logger = logger;
        }

        public async Task<StrategicPivot> InitiatePivotAsync(
            PivotType type,
            PivotRationale rationale,
            string description,
            Guid initiatedBy,
            DateTime initiatedDate,
            DateTime targetCompletionDate,
            Guid? tenantId)
        {
            try
            {
                var pivot = StrategicPivot.Create(
                    GuidGenerator.Create(),
                    type,
                    rationale,
                    description,
                    initiatedBy,
                    initiatedDate,
                    targetCompletionDate,
                    tenantId
                );

                await _pivotRepository.InsertAsync(pivot);

                _logger.LogInformation("Strategic pivot initiated: {PivotId} - Type: {Type}", 
                    pivot.Id, pivot.Type);

                return pivot;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to initiate strategic pivot of type {Type}", type);
                throw;
            }
        }

        public async Task<ValidationResult> ValidatePivotRationaleAsync(PivotRationale rationale)
        {
            var result = new ValidationResult();

            // Validate reason
            if (string.IsNullOrWhiteSpace(rationale.Reason) || rationale.Reason.Length < 10)
                result.AddError("Pivot reason must be at least 10 characters long");

            // Validate expected outcome
            if (string.IsNullOrWhiteSpace(rationale.ExpectedOutcome))
                result.AddError("Expected outcome is required");

            // Validate insights
            if (!rationale.KeyInsights.Any())
                result.AddWarning("No key insights provided - consider adding supporting data");

            // Validate data points
            if (!rationale.DataPoints.Any())
                result.AddWarning("No data points provided - pivot decisions should be data-driven");

            // Validate success metrics
            if (!rationale.SuccessMetrics.Any())
                result.AddError("Success metrics are required to measure pivot effectiveness");

            return result;
        }

        public async Task<List<ImpactMetricDefinition>> SuggestImpactMetricsAsync(
            PivotType type,
            string description)
        {
            var suggestions = new List<ImpactMetricDefinition>();

            var commonMetrics = GetCommonMetricsForPivotType(type);
            var customMetrics = await ExtractMetricsFromDescription(description);

            suggestions.AddRange(commonMetrics);
            suggestions.AddRange(customMetrics);

            return suggestions;
        }

        public async Task LogPivotMilestoneAsync(
            StrategicPivot pivot,
            string milestone,
            DateTime milestoneDate,
            string notes)
        {
            _logger.LogInformation("Pivot milestone achieved: {PivotId} - {Milestone} on {Date}",
                pivot.Id, milestone, milestoneDate);

            // This could be stored as events or in a separate milestone tracking entity
            // For now, we'll just log it
        }

        public async Task<List<StrategicPivot>> GetRelatedPivotsAsync(PivotType type, Guid? tenantId)
        {
            var specification = new PivotTypeSpecification(type);
            
            if (tenantId.HasValue)
            {
                // Combine with tenant specification
                return await _pivotRepository.GetListAsync(p => p.Type == type && p.TenantId == tenantId);
            }

            return await _pivotRepository.GetListAsync(specification);
        }

        private List<ImpactMetricDefinition> GetCommonMetricsForPivotType(PivotType type)
        {
            return type switch
            {
                PivotType.CustomerSegment => new List<ImpactMetricDefinition>
                {
                    CreateMetricSuggestion("Customer Acquisition Rate", "Rate of new customer acquisition", "customers/month"),
                    CreateMetricSuggestion("Customer Satisfaction Score", "Customer satisfaction rating", "score (1-10)"),
                    CreateMetricSuggestion("Market Share", "Percentage of target market captured", "percentage")
                },
                PivotType.Revenue => new List<ImpactMetricDefinition>
                {
                    CreateMetricSuggestion("Monthly Recurring Revenue", "Monthly recurring revenue", "currency"),
                    CreateMetricSuggestion("Average Revenue Per User", "Average revenue per user", "currency"),
                    CreateMetricSuggestion("Revenue Growth Rate", "Month-over-month revenue growth", "percentage")
                },
                PivotType.Solution => new List<ImpactMetricDefinition>
                {
                    CreateMetricSuggestion("Product Adoption Rate", "Rate of product feature adoption", "percentage"),
                    CreateMetricSuggestion("User Engagement Score", "Level of user engagement", "score"),
                    CreateMetricSuggestion("Problem Resolution Rate", "Rate of problem resolution", "percentage")
                },
                _ => new List<ImpactMetricDefinition>
                {
                    CreateMetricSuggestion("Success Indicator", "Primary success indicator", "units"),
                    CreateMetricSuggestion("Performance Metric", "Key performance metric", "units")
                }
            };
        }

        private async Task<List<ImpactMetricDefinition>> ExtractMetricsFromDescription(string description)
        {
            // This would typically use NLP/ML to extract potential metrics from description
            // For now, returning empty list
            return new List<ImpactMetricDefinition>();
        }

        private ImpactMetricDefinition CreateMetricSuggestion(string name, string description, string unit)
        {
            return new ImpactMetricDefinition(
                Guid.NewGuid(),
                Guid.Empty, // Will be set when associated with a pivot
                name,
                description,
                unit,
                "Manual measurement" // Default measurement method
            );
        }
    }
}
