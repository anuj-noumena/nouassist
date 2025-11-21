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
using Volo.Abp.Domain.Services;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Services
{
    /// <summary>
    /// Domain service for hypothesis tracking and validation
    /// </summary>
    public class HypothesisTrackingService : DomainService, IHypothesisTrackingService
    {
        private readonly IBusinessModelCanvasRepository _canvasRepository;
        private readonly ILogger<HypothesisTrackingService> _logger;

        public HypothesisTrackingService(
            IBusinessModelCanvasRepository canvasRepository,
            ILogger<HypothesisTrackingService> logger)
        {
            _canvasRepository = canvasRepository;
            _logger = logger;
        }

        public async Task<AssociatedHypothesis> CreateHypothesisAsync(
            BusinessModelCanvas canvas,
            CanvasComponentType componentType,
            string assumption,
            Guid createdBy,
            DateTime creationTime)
        {
            try
            {
                var hypothesis = canvas.AddHypothesis(componentType, assumption, createdBy, creationTime);
                await _canvasRepository.UpdateAsync(canvas);

                _logger.LogInformation("Hypothesis created: {HypothesisId} for component {ComponentType} in canvas {CanvasId}",
                    hypothesis.Id, componentType, canvas.Id);

                return hypothesis;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create hypothesis for component {ComponentType} in canvas {CanvasId}",
                    componentType, canvas.Id);
                throw;
            }
        }

        public async Task<ValidationResult> ValidateHypothesisTestAsync(
            AssociatedHypothesis hypothesis,
            string testDescription,
            string testResult,
            ValidationStatus finalStatus)
        {
            var result = new ValidationResult();

            // Validate test description
            if (string.IsNullOrWhiteSpace(testDescription) || testDescription.Length < 10)
                result.AddError("Test description must be at least 10 characters long");

            // Validate test result
            if (string.IsNullOrWhiteSpace(testResult) || testResult.Length < 5)
                result.AddError("Test result must be at least 5 characters long");

            // Validate final status
            if (finalStatus == ValidationStatus.NotTested || finalStatus == ValidationStatus.InProgress)
                result.AddError("Final status must be a conclusive status (Validated, Invalidated, or NeedsMoreTesting)");

            // Business logic validation
            if (finalStatus == ValidationStatus.Validated && !testResult.ToLower().Contains("success"))
                result.AddWarning("Test result should indicate success for validated hypothesis");

            if (finalStatus == ValidationStatus.Invalidated && !testResult.ToLower().Contains("fail"))
                result.AddWarning("Test result should indicate failure for invalidated hypothesis");

            return result;
        }

        public async Task<List<AssociatedHypothesis>> GetPendingHypothesesAsync(BusinessModelCanvas canvas)
        {
            var pendingHypotheses = new List<AssociatedHypothesis>();

            foreach (var component in canvas.Components)
            {
                var pending = component.Hypotheses
                    .Where(h => h.Details.Status == ValidationStatus.NotTested || 
                               h.Details.Status == ValidationStatus.InProgress)
                    .ToList();
                
                pendingHypotheses.AddRange(pending);
            }

            return pendingHypotheses.OrderBy(h => h.CreatedAt).ToList();
        }

        public async Task<double> CalculateValidationProgressAsync(BusinessModelCanvas canvas)
        {
            return canvas.GetOverallValidationProgress();
        }

        public async Task<List<string>> GenerateTestSuggestionsAsync(AssociatedHypothesis hypothesis)
        {
            var suggestions = new List<string>();

            switch (hypothesis.ComponentType)
            {
                case CanvasComponentType.ValuePropositions:
                    suggestions.AddRange(new[]
                    {
                        "Conduct customer interviews to validate value propositions",
                        "A/B test different value proposition messaging",
                        "Survey customers about perceived value",
                        "Analyze customer behavior and usage patterns"
                    });
                    break;

                case CanvasComponentType.CustomerSegments:
                    suggestions.AddRange(new[]
                    {
                        "Conduct market research to validate segment characteristics",
                        "Interview potential customers in each segment",
                        "Analyze customer data to identify patterns",
                        "Test marketing campaigns targeting specific segments"
                    });
                    break;

                case CanvasComponentType.Channels:
                    suggestions.AddRange(new[]
                    {
                        "Run pilot campaigns through proposed channels",
                        "Measure conversion rates across different channels",
                        "Test customer acquisition cost per channel",
                        "Survey customers about preferred channels"
                    });
                    break;

                case CanvasComponentType.RevenueStreams:
                    suggestions.AddRange(new[]
                    {
                        "Test different pricing models with small customer groups",
                        "Analyze willingness to pay through surveys",
                        "Run pricing experiments",
                        "Monitor competitor pricing strategies"
                    });
                    break;

                default:
                    suggestions.AddRange(new[]
                    {
                        "Design and run specific experiments",
                        "Collect and analyze relevant data",
                        "Interview stakeholders",
                        "Monitor key metrics over time"
                    });
                    break;
            }

            return suggestions;
        }
    }
}
