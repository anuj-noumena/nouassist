using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Aggregates;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Enums;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Interfaces.Services;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Repositories;
using Volo.Abp.Domain.Services;

namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Services
{
    /// <summary>
    /// Domain service for tracking and analyzing hypothesis validation progress
    /// </summary>
    public class HypothesisValidationTracker : DomainService, IHypothesisValidationTracker
    {
        private readonly IProblemHypothesisRepository _hypothesisRepository;
        private readonly ILogger<HypothesisValidationTracker> _logger;

        public HypothesisValidationTracker(
            IProblemHypothesisRepository hypothesisRepository,
            ILogger<HypothesisValidationTracker> logger)
        {
            _hypothesisRepository = hypothesisRepository;
            _logger = logger;
        }

        public async Task<ValidationProgress> CalculateValidationProgressAsync(ProblemHypothesis hypothesis)
        {
            try
            {
                var totalAttempts = hypothesis.ValidationAttempts.Count;
                var completedAttempts = hypothesis.GetCompletedValidationCount();
                var completionPercentage = totalAttempts > 0 ? (decimal)completedAttempts / totalAttempts * 100 : 0;

                var completedMethods = hypothesis.ValidationAttempts
                    .Where(v => v.Status == ValidationStatus.Validated ||
                               v.Status == ValidationStatus.Invalidated ||
                               v.Status == ValidationStatus.Inconclusive)
                    .Select(v => v.Method)
                    .Distinct()
                    .ToList();

                var pendingMethods = hypothesis.ValidationAttempts
                    .Where(v => v.Status == ValidationStatus.NotStarted ||
                               v.Status == ValidationStatus.InProgress)
                    .Select(v => v.Method)
                    .Distinct()
                    .ToList();

                DateTime? estimatedCompletion = null;
                if (pendingMethods.Any())
                {
                    var averageDurationDays = CalculateAverageValidationDuration(hypothesis);
                    estimatedCompletion = DateTime.UtcNow.AddDays(averageDurationDays * pendingMethods.Count);
                }

                return new ValidationProgress
                {
                    HypothesisId = hypothesis.Id,
                    CompletionPercentage = completionPercentage,
                    PlannedAttempts = totalAttempts,
                    CompletedAttempts = completedAttempts,
                    CurrentStatus = hypothesis.Status,
                    CompletedMethods = completedMethods,
                    PendingMethods = pendingMethods,
                    EstimatedCompletionDate = estimatedCompletion
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to calculate validation progress for hypothesis {HypothesisId}", hypothesis.Id);
                throw;
            }
        }

        public async Task<List<ValidationRecommendation>> GetValidationRecommendationsAsync(ProblemHypothesis hypothesis)
        {
            try
            {
                var recommendations = new List<ValidationRecommendation>();

                // Check if hypothesis has sufficient validation attempts
                if (hypothesis.ValidationAttempts.Count < 2)
                {
                    recommendations.Add(new ValidationRecommendation
                    {
                        Type = "Increase Validation Coverage",
                        Description = "Add more validation attempts to strengthen evidence",
                        Priority = FeaturePriority.High,
                        Rationale = "Multiple validation methods provide more robust evidence",
                        SuggestedMethods = new List<string> { "Customer Interviews", "A/B Testing", "Surveys" },
                        EstimatedEffortDays = 14
                    });
                }

                // Check if validation is stalled
                var stalledAttempts = hypothesis.ValidationAttempts
                    .Where(v => v.Status == ValidationStatus.InProgress &&
                               v.ActualStartDate.HasValue &&
                               (DateTime.UtcNow - v.ActualStartDate.Value).TotalDays > 30)
                    .ToList();

                if (stalledAttempts.Any())
                {
                    recommendations.Add(new ValidationRecommendation
                    {
                        Type = "Address Stalled Validation",
                        Description = "Several validation attempts have been in progress for over 30 days",
                        Priority = FeaturePriority.Medium,
                        Rationale = "Stalled validations may indicate resource or methodology issues",
                        SuggestedMethods = new List<string> { "Resource Reallocation", "Method Review" },
                        EstimatedEffortDays = 7
                    });
                }

                // Check validation method diversity
                var methods = hypothesis.ValidationAttempts.Select(v => v.Method).Distinct().ToList();
                if (methods.Count < 3 && hypothesis.ValidationAttempts.Count >= 3)
                {
                    recommendations.Add(new ValidationRecommendation
                    {
                        Type = "Diversify Validation Methods",
                        Description = "Use different validation methods for more comprehensive evidence",
                        Priority = FeaturePriority.Medium,
                        Rationale = "Method diversity reduces validation bias and increases confidence",
                        SuggestedMethods = GetSuggestedMethods(methods),
                        EstimatedEffortDays = 10
                    });
                }

                // Check if hypothesis needs quantitative validation
                var hasQuantitativeValidation = hypothesis.ValidationAttempts
                    .Any(v => v.Metrics.Any());

                if (!hasQuantitativeValidation)
                {
                    recommendations.Add(new ValidationRecommendation
                    {
                        Type = "Add Quantitative Validation",
                        Description = "Include measurable metrics in validation attempts",
                        Priority = FeaturePriority.Medium,
                        Rationale = "Quantitative data provides objective validation evidence",
                        SuggestedMethods = new List<string> { "Metrics Definition", "A/B Testing", "Usage Analytics" },
                        EstimatedEffortDays = 5
                    });
                }

                return recommendations;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate validation recommendations for hypothesis {HypothesisId}", hypothesis.Id);
                throw;
            }
        }

        public async Task<ValidationPortfolioMetrics> AnalyzeValidationPortfolioAsync(List<ProblemHypothesis> hypotheses)
        {
            try
            {
                if (!hypotheses.Any())
                {
                    return new ValidationPortfolioMetrics
                    {
                        TotalHypotheses = 0,
                        StatusDistribution = new Dictionary<ValidationStatus, int>(),
                        OverallCompletionRate = 0,
                        ValidationSuccessRate = 0,
                        CommonValidationMethods = new List<string>(),
                        AverageDaysToValidation = 0,
                        Trends = new List<ValidationTrend>()
                    };
                }

                var statusDistribution = hypotheses
                    .GroupBy(h => h.Status)
                    .ToDictionary(g => g.Key, g => g.Count());

                var completedHypotheses = hypotheses
                    .Where(h => h.Status == ValidationStatus.Validated ||
                               h.Status == ValidationStatus.Invalidated ||
                               h.Status == ValidationStatus.Inconclusive)
                    .ToList();

                var overallCompletionRate = (decimal)completedHypotheses.Count / hypotheses.Count * 100;

                var validatedCount = hypotheses.Count(h => h.Status == ValidationStatus.Validated);
                var validationSuccessRate = completedHypotheses.Any() ? 
                    (decimal)validatedCount / completedHypotheses.Count * 100 : 0;

                var allMethods = hypotheses
                    .SelectMany(h => h.ValidationAttempts)
                    .GroupBy(v => v.Method)
                    .OrderByDescending(g => g.Count())
                    .Select(g => g.Key)
                    .Take(5)
                    .ToList();

                var averageDaysToValidation = CalculateAverageValidationTime(completedHypotheses);

                var trends = CalculateValidationTrends(hypotheses);

                return new ValidationPortfolioMetrics
                {
                    TotalHypotheses = hypotheses.Count,
                    StatusDistribution = statusDistribution,
                    OverallCompletionRate = overallCompletionRate,
                    ValidationSuccessRate = validationSuccessRate,
                    CommonValidationMethods = allMethods,
                    AverageDaysToValidation = averageDaysToValidation,
                    Trends = trends
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to analyze validation portfolio");
                throw;
            }
        }

        public async Task<ValidationOutcomePrediction> PredictValidationOutcomeAsync(ProblemHypothesis hypothesis)
        {
            try
            {
                // Simple prediction algorithm based on current progress and historical patterns
                var completedAttempts = hypothesis.ValidationAttempts
                    .Where(v => v.Status != ValidationStatus.NotStarted && v.Status != ValidationStatus.InProgress)
                    .ToList();

                if (!completedAttempts.Any())
                {
                    return new ValidationOutcomePrediction
                    {
                        PredictedOutcome = ValidationStatus.InProgress,
                        Confidence = 0.3m,
                        Reasoning = "Insufficient validation data for prediction",
                        RiskFactors = new List<string> { "No completed validations" },
                        SuccessFactors = new List<string>(),
                        EstimatedCompletionDate = DateTime.UtcNow.AddDays(30)
                    };
                }

                var validatedCount = completedAttempts.Count(v => v.Status == ValidationStatus.Validated);
                var invalidatedCount = completedAttempts.Count(v => v.Status == ValidationStatus.Invalidated);
                var inconclusiveCount = completedAttempts.Count(v => v.Status == ValidationStatus.Inconclusive);

                ValidationStatus predictedOutcome;
                decimal confidence;
                string reasoning;

                if (validatedCount > invalidatedCount + inconclusiveCount)
                {
                    predictedOutcome = ValidationStatus.Validated;
                    confidence = (decimal)validatedCount / completedAttempts.Count;
                    reasoning = $"Most validation attempts ({validatedCount}/{completedAttempts.Count}) resulted in validation";
                }
                else if (invalidatedCount > validatedCount + inconclusiveCount)
                {
                    predictedOutcome = ValidationStatus.Invalidated;
                    confidence = (decimal)invalidatedCount / completedAttempts.Count;
                    reasoning = $"Most validation attempts ({invalidatedCount}/{completedAttempts.Count}) resulted in invalidation";
                }
                else
                {
                    predictedOutcome = ValidationStatus.Inconclusive;
                    confidence = 0.5m;
                    reasoning = "Mixed validation results suggest inconclusive outcome";
                }

                var riskFactors = IdentifyRiskFactors(hypothesis);
                var successFactors = IdentifySuccessFactors(hypothesis);
                var estimatedCompletion = EstimateCompletionDate(hypothesis);

                return new ValidationOutcomePrediction
                {
                    PredictedOutcome = predictedOutcome,
                    Confidence = confidence,
                    Reasoning = reasoning,
                    RiskFactors = riskFactors,
                    SuccessFactors = successFactors,
                    EstimatedCompletionDate = estimatedCompletion
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to predict validation outcome for hypothesis {HypothesisId}", hypothesis.Id);
                throw;
            }
        }

        public async Task<ValidationStatusReport> GenerateStatusReportAsync(List<ProblemHypothesis> hypotheses)
        {
            try
            {
                var portfolioMetrics = await AnalyzeValidationPortfolioAsync(hypotheses);
                
                var hypothesesProgress = new List<ValidationProgress>();
                foreach (var hypothesis in hypotheses)
                {
                    var progress = await CalculateValidationProgressAsync(hypothesis);
                    hypothesesProgress.Add(progress);
                }

                var topRecommendations = new List<ValidationRecommendation>();
                foreach (var hypothesis in hypotheses.Take(5)) // Top 5 for summary
                {
                    var recommendations = await GetValidationRecommendationsAsync(hypothesis);
                    topRecommendations.AddRange(recommendations);
                }

                topRecommendations = topRecommendations
                    .OrderByDescending(r => r.Priority)
                    .Take(10)
                    .ToList();

                var keyFindings = GenerateKeyFindings(portfolioMetrics, hypothesesProgress);
                var actionItems = GenerateActionItems(topRecommendations, portfolioMetrics);

                return new ValidationStatusReport
                {
                    GeneratedDate = DateTime.UtcNow,
                    PortfolioMetrics = portfolioMetrics,
                    HypothesesProgress = hypothesesProgress,
                    TopRecommendations = topRecommendations,
                    KeyFindings = keyFindings,
                    ActionItems = actionItems
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate validation status report");
                throw;
            }
        }

        private int CalculateAverageValidationDuration(ProblemHypothesis hypothesis)
        {
            var completedAttempts = hypothesis.ValidationAttempts
                .Where(v => v.CompletedDate.HasValue && v.ActualStartDate.HasValue)
                .ToList();

            if (!completedAttempts.Any()) return 14; // Default 2 weeks

            var totalDays = completedAttempts
                .Sum(v => (v.CompletedDate.Value - v.ActualStartDate.Value).TotalDays);

            return (int)(totalDays / completedAttempts.Count);
        }

        private List<string> GetSuggestedMethods(List<string> existingMethods)
        {
            var allMethods = new List<string>
            {
                "Customer Interviews", "Surveys", "A/B Testing", "Usability Testing",
                "Analytics Review", "Competitor Analysis", "Expert Review", "Prototyping"
            };

            return allMethods.Except(existingMethods).Take(3).ToList();
        }

        private int CalculateAverageValidationTime(List<ProblemHypothesis> completedHypotheses)
        {
            if (!completedHypotheses.Any()) return 0;

            var totalDays = 0;
            var validatedCount = 0;

            foreach (var hypothesis in completedHypotheses.Where(h => h.ValidatedDate.HasValue))
            {
                var firstAttempt = hypothesis.ValidationAttempts
                    .Where(v => v.ActualStartDate.HasValue)
                    .OrderBy(v => v.ActualStartDate)
                    .FirstOrDefault();

                if (firstAttempt != null)
                {
                    totalDays += (int)(hypothesis.ValidatedDate.Value - firstAttempt.ActualStartDate.Value).TotalDays;
                    validatedCount++;
                }
            }

            return validatedCount > 0 ? totalDays / validatedCount : 0;
        }

        private List<ValidationTrend> CalculateValidationTrends(List<ProblemHypothesis> hypotheses)
        {
            // Simple trend calculation - can be enhanced with more sophisticated analysis
            var trends = new List<ValidationTrend>();

            // Validation completion rate trend
            var recentHypotheses = hypotheses.Where(h => h.CreationTime >= DateTime.UtcNow.AddDays(-90)).ToList();
            var olderHypotheses = hypotheses.Where(h => h.CreationTime < DateTime.UtcNow.AddDays(-90)).ToList();

            if (recentHypotheses.Any() && olderHypotheses.Any())
            {
                var recentCompletionRate = (decimal)recentHypotheses.Count(h => h.Status != ValidationStatus.NotStarted && h.Status != ValidationStatus.InProgress) / recentHypotheses.Count * 100;
                var olderCompletionRate = (decimal)olderHypotheses.Count(h => h.Status != ValidationStatus.NotStarted && h.Status != ValidationStatus.InProgress) / olderHypotheses.Count * 100;
                
                var change = recentCompletionRate - olderCompletionRate;
                
                trends.Add(new ValidationTrend
                {
                    Metric = "Completion Rate",
                    Period = "Last 90 days vs Previous",
                    Direction = change > 0 ? "Improving" : change < 0 ? "Declining" : "Stable",
                    PercentageChange = change,
                    Description = $"Validation completion rate has {(change > 0 ? "increased" : change < 0 ? "decreased" : "remained stable")} by {Math.Abs(change):F1}%"
                });
            }

            return trends;
        }

        private List<string> IdentifyRiskFactors(ProblemHypothesis hypothesis)
        {
            var riskFactors = new List<string>();

            if (hypothesis.ValidationAttempts.Count < 2)
                riskFactors.Add("Limited validation methods");

            if (hypothesis.ValidationAttempts.Any(v => v.Status == ValidationStatus.InProgress && 
                v.ActualStartDate.HasValue && (DateTime.UtcNow - v.ActualStartDate.Value).TotalDays > 30))
                riskFactors.Add("Stalled validation attempts");

            if (!hypothesis.ValidationAttempts.Any(v => v.Metrics.Any()))
                riskFactors.Add("Lack of quantitative validation");

            if (hypothesis.RelatedCustomerSegmentId == null)
                riskFactors.Add("No specific customer segment defined");

            return riskFactors;
        }

        private List<string> IdentifySuccessFactors(ProblemHypothesis hypothesis)
        {
            var successFactors = new List<string>();

            if (hypothesis.ValidationAttempts.Count >= 3)
                successFactors.Add("Multiple validation methods planned");

            if (hypothesis.ValidationAttempts.Any(v => v.Metrics.Any()))
                successFactors.Add("Quantitative validation metrics defined");

            if (hypothesis.RelatedCustomerSegmentId.HasValue)
                successFactors.Add("Clear customer segment identified");

            if (hypothesis.ValidationAttempts.Count(v => v.Status == ValidationStatus.Validated) > 
                hypothesis.ValidationAttempts.Count(v => v.Status == ValidationStatus.Invalidated))
                successFactors.Add("Positive validation trend");

            return successFactors;
        }

        private DateTime EstimateCompletionDate(ProblemHypothesis hypothesis)
        {
            var pendingAttempts = hypothesis.ValidationAttempts
                .Where(v => v.Status == ValidationStatus.NotStarted || v.Status == ValidationStatus.InProgress)
                .ToList();

            if (!pendingAttempts.Any())
                return DateTime.UtcNow;

            var averageDuration = CalculateAverageValidationDuration(hypothesis);
            var remainingDays = averageDuration * pendingAttempts.Count;

            return DateTime.UtcNow.AddDays(remainingDays);
        }

        private List<string> GenerateKeyFindings(ValidationPortfolioMetrics metrics, List<ValidationProgress> progress)
        {
            var findings = new List<string>();

            findings.Add($"Portfolio contains {metrics.TotalHypotheses} hypotheses with {metrics.OverallCompletionRate:F1}% completion rate");
            findings.Add($"Validation success rate is {metrics.ValidationSuccessRate:F1}%");

            if (metrics.AverageDaysToValidation > 0)
                findings.Add($"Average validation time is {metrics.AverageDaysToValidation} days");

            var stalledCount = progress.Count(p => p.CompletionPercentage < 50 && 
                (p.CurrentStatus == ValidationStatus.InProgress || p.CurrentStatus == ValidationStatus.NotStarted));
            
            if (stalledCount > 0)
                findings.Add($"{stalledCount} hypotheses have stalled validation progress");

            return findings;
        }

        private List<string> GenerateActionItems(List<ValidationRecommendation> recommendations, ValidationPortfolioMetrics metrics)
        {
            var actionItems = new List<string>();

            var highPriorityRecs = recommendations.Where(r => r.Priority == FeaturePriority.Critical || r.Priority == FeaturePriority.High).ToList();
            
            if (highPriorityRecs.Any())
                actionItems.Add($"Address {highPriorityRecs.Count} high-priority validation issues");

            if (metrics.OverallCompletionRate < 70)
                actionItems.Add("Focus on completing pending validation attempts");

            if (metrics.ValidationSuccessRate < 60)
                actionItems.Add("Review and improve validation methodologies");

            if (metrics.AverageDaysToValidation > 45)
                actionItems.Add("Optimize validation processes to reduce time to completion");

            return actionItems;
        }
    }
}
