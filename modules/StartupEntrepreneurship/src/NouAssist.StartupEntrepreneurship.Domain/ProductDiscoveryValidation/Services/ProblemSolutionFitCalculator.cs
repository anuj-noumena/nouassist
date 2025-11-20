namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Services
{
    /// <summary>
    /// Domain service for calculating problem-solution fit metrics
    /// </summary>
    public class ProblemSolutionFitCalculator : DomainService, IProblemSolutionFitCalculator
    {
        private readonly IProblemHypothesisRepository _hypothesisRepository;
        private readonly IUserFeedbackSessionRepository _feedbackRepository;
        private readonly ILogger<ProblemSolutionFitCalculator> _logger;

        public ProblemSolutionFitCalculator(
            IProblemHypothesisRepository hypothesisRepository,
            IUserFeedbackSessionRepository feedbackRepository,
            ILogger<ProblemSolutionFitCalculator> logger)
        {
            _hypothesisRepository = hypothesisRepository;
            _feedbackRepository = feedbackRepository;
            _logger = logger;
        }

        public async Task<ProblemSolutionFitScore> CalculateFitScoreAsync(
            ProblemHypothesis hypothesis, 
            List<UserFeedbackSession> feedbackSessions)
        {
            try
            {
                // Calculate individual component scores
                var problemClarity = CalculateProblemClarity(hypothesis);
                var solutionEffectiveness = CalculateSolutionEffectiveness(hypothesis, feedbackSessions);
                var customerValidation = CalculateCustomerValidation(hypothesis);
                var marketDemand = CalculateMarketDemand(feedbackSessions);

                var fitMetric = new ProblemSolutionFitMetric(
                    problemClarity,
                    solutionEffectiveness,
                    customerValidation,
                    marketDemand,
                    "Calculated from validation data and user feedback"
                );

                var overallScore = fitMetric.CalculateOverallFit();
                var fitLevel = fitMetric.GetFitLevel();

                var componentScores = new Dictionary<string, decimal>
                {
                    ["Problem Clarity"] = problemClarity,
                    ["Solution Effectiveness"] = solutionEffectiveness,
                    ["Customer Validation"] = customerValidation,
                    ["Market Demand"] = marketDemand
                };

                var strengthAreas = IdentifyStrengthAreas(componentScores);
                var weaknessAreas = IdentifyWeaknessAreas(componentScores);
                var confidence = CalculateConfidence(hypothesis, feedbackSessions);

                return new ProblemSolutionFitScore
                {
                    HypothesisId = hypothesis.Id,
                    OverallScore = overallScore,
                    DetailedMetrics = fitMetric,
                    FitLevel = fitLevel,
                    ComponentScores = componentScores,
                    StrengthAreas = strengthAreas,
                    WeaknessAreas = weaknessAreas,
                    Confidence = confidence,
                    CalculatedDate = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to calculate fit score for hypothesis {HypothesisId}", hypothesis.Id);
                throw;
            }
        }

        public async Task<FitAnalysisReport> AnalyzeFitAcrossHypothesesAsync(List<ProblemHypothesis> hypotheses)
        {
            try
            {
                var hypothesesScores = new List<ProblemSolutionFitScore>();

                foreach (var hypothesis in hypotheses)
                {
                    var feedbackSessions = await _feedbackRepository.GetListAsync(s => true); // Simplified - in real implementation would filter by hypothesis
                    var fitScore = await CalculateFitScoreAsync(hypothesis, feedbackSessions);
                    hypothesesScores.Add(fitScore);
                }

                if (!hypothesesScores.Any())
                {
                    return new FitAnalysisReport
                    {
                        HypothesesScores = new List<ProblemSolutionFitScore>(),
                        AverageScore = 0,
                        BestPerforming = null,
                        WorstPerforming = null,
                        FitLevelDistribution = new Dictionary<string, int>(),
                        PortfolioInsights = new List<string>(),
                        RecommendedActions = new List<string>()
                    };
                }

                var averageScore = hypothesesScores.Average(s => s.OverallScore);
                var bestPerforming = hypothesesScores.OrderByDescending(s => s.OverallScore).First();
                var worstPerforming = hypothesesScores.OrderBy(s => s.OverallScore).First();

                var fitLevelDistribution = hypothesesScores
                    .GroupBy(s => s.FitLevel)
                    .ToDictionary(g => g.Key, g => g.Count());

                var insights = GeneratePortfolioInsights(hypothesesScores);
                var actions = GeneratePortfolioActions(hypothesesScores);

                return new FitAnalysisReport
                {
                    HypothesesScores = hypothesesScores,
                    AverageScore = averageScore,
                    BestPerforming = bestPerforming,
                    WorstPerforming = worstPerforming,
                    FitLevelDistribution = fitLevelDistribution,
                    PortfolioInsights = insights,
                    RecommendedActions = actions
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to analyze fit across hypotheses");
                throw;
            }
        }

        public async Task<FitEvolutionAnalysis> AnalyzeFitEvolutionAsync(ProblemHypothesis hypothesis)
        {
            try
            {
                // Get historical fit calculations (simplified - would need to store historical data)
                var evolutionPoints = new List<FitEvolutionPoint>();

                // Create evolution points from validation attempts
                foreach (var attempt in hypothesis.ValidationAttempts.OrderBy(v => v.ActualStartDate))
                {
                    if (attempt.ActualStartDate.HasValue)
                    {
                        // Simulate evolution based on validation results
                        var score = attempt.Status switch
                        {
                            ValidationStatus.Validated => 80m,
                            ValidationStatus.Invalidated => 30m,
                            ValidationStatus.Inconclusive => 50m,
                            _ => 50m
                        };

                        evolutionPoints.Add(new FitEvolutionPoint
                        {
                            Date = attempt.ActualStartDate.Value,
                            Score = score,
                            Event = $"Validation: {attempt.Method}",
                            Notes = attempt.Results ?? "Validation completed"
                        });
                    }
                }

                if (!evolutionPoints.Any())
                {
                    return new FitEvolutionAnalysis
                    {
                        HypothesisId = hypothesis.Id,
                        EvolutionPoints = new List<FitEvolutionPoint>(),
                        Trend = "No Data",
                        TrendStrength = 0,
                        InfluencingFactors = new List<string>(),
                        Milestones = new List<FitMilestone>(),
                        Projection = null
                    };
                }

                var trend = CalculateTrend(evolutionPoints);
                var trendStrength = CalculateTrendStrength(evolutionPoints);
                var influencingFactors = IdentifyInfluencingFactors(hypothesis);
                var milestones = IdentifyMilestones(evolutionPoints);
                var projection = GenerateProjection(evolutionPoints, trend);

                return new FitEvolutionAnalysis
                {
                    HypothesisId = hypothesis.Id,
                    EvolutionPoints = evolutionPoints,
                    Trend = trend,
                    TrendStrength = trendStrength,
                    InfluencingFactors = influencingFactors,
                    Milestones = milestones,
                    Projection = projection
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to analyze fit evolution for hypothesis {HypothesisId}", hypothesis.Id);
                throw;
            }
        }

        public async Task<List<FitImprovementRecommendation>> GenerateFitRecommendationsAsync(ProblemHypothesis hypothesis)
        {
            try
            {
                var feedbackSessions = await _feedbackRepository.GetListAsync(s => true);
                var fitScore = await CalculateFitScoreAsync(hypothesis, feedbackSessions);
                var recommendations = new List<FitImprovementRecommendation>();

                // Analyze each component and generate recommendations
                foreach (var component in fitScore.ComponentScores)
                {
                    if (component.Value < 70) // Below threshold
                    {
                        var recommendation = GenerateComponentRecommendation(component.Key, component.Value, hypothesis);
                        if (recommendation != null)
                            recommendations.Add(recommendation);
                    }
                }

                // Overall recommendations based on fit level
                if (fitScore.OverallScore < 60)
                {
                    recommendations.Add(new FitImprovementRecommendation
                    {
                        Area = "Overall Fit",
                        Recommendation = "Conduct comprehensive review of problem definition and solution approach",
                        PotentialImprovement = 70 - fitScore.OverallScore,
                        Priority = FeaturePriority.Critical,
                        ImplementationEffort = 21,
                        ActionSteps = new List<string>
                        {
                            "Revisit customer research",
                            "Validate core assumptions",
                            "Consider solution pivots",
                            "Increase validation frequency"
                        },
                        SuccessCriteria = "Achieve fit score above 70%"
                    });
                }

                return recommendations.OrderByDescending(r => r.PotentialImprovement).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate fit recommendations for hypothesis {HypothesisId}", hypothesis.Id);
                throw;
            }
        }

        public async Task<FitBenchmarkResult> BenchmarkFitAsync(ProblemSolutionFitScore fitScore, string industry = null)
        {
            try
            {
                // Benchmark data (in real implementation, this would come from industry data)
                var industryAverage = 65m;
                var topQuartile = 80m;
                var bottomQuartile = 45m;

                var percentile = CalculatePercentile(fitScore.OverallScore, industryAverage, topQuartile, bottomQuartile);
                var interpretation = GenerateBenchmarkInterpretation(fitScore.OverallScore, percentile);
                var insights = GenerateBenchmarkInsights(fitScore, industryAverage);
                var opportunities = GenerateBenchmarkOpportunities(fitScore, topQuartile);

                return new FitBenchmarkResult
                {
                    YourScore = fitScore.OverallScore,
                    IndustryAverage = industryAverage,
                    TopQuartile = topQuartile,
                    BottomQuartile = bottomQuartile,
                    Percentile = percentile,
                    Interpretation = interpretation,
                    BenchmarkInsights = insights,
                    ImprovementOpportunities = opportunities
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to benchmark fit score");
                throw;
            }
        }

        #region Private Methods

        private decimal CalculateProblemClarity(ProblemHypothesis hypothesis)
        {
            var score = 50m; // Base score

            // Factors that improve problem clarity
            if (hypothesis.RelatedCustomerSegmentId.HasValue)
                score += 15; // Clear target segment

            if (hypothesis.ValidationAttempts.Any(v => v.Method.Contains("Interview")))
                score += 20; // Customer interviews conducted

            if (hypothesis.Statement.Problem.Length > 50)
                score += 10; // Detailed problem statement

            if (hypothesis.ValidationAttempts.Count(v => v.Status == ValidationStatus.Validated) >= 2)
                score += 15; // Multiple validations

            return Math.Min(100, score);
        }

        private decimal CalculateSolutionEffectiveness(ProblemHypothesis hypothesis, List<UserFeedbackSession> feedbackSessions)
        {
            if (!feedbackSessions.Any()) return 50m;

            // Calculate based on user feedback satisfaction
            var allFeedback = feedbackSessions.SelectMany(s => s.FeedbackData).ToList();
            if (!allFeedback.Any()) return 50m;

            var averageSatisfaction = allFeedback.Average(f => f.Rating.PercentageScore());
            
            // Adjust for multiple feedback sessions (more data = higher confidence)
            var sessionBonus = Math.Min(20m, feedbackSessions.Count * 4);
            
            return Math.Min(100, averageSatisfaction + sessionBonus);
        }

        private decimal CalculateCustomerValidation(ProblemHypothesis hypothesis)
        {
            var validationAttempts = hypothesis.ValidationAttempts;
            if (!validationAttempts.Any()) return 0m;

            var completedAttempts = validationAttempts.Where(v => 
                v.Status == ValidationStatus.Validated ||
                v.Status == ValidationStatus.Invalidated ||
                v.Status == ValidationStatus.Inconclusive).ToList();

            if (!completedAttempts.Any()) return 25m;

            var validatedCount = completedAttempts.Count(v => v.Status == ValidationStatus.Validated);
            var validationRate = (decimal)validatedCount / completedAttempts.Count;

            // Base score from validation rate
            var score = validationRate * 70;

            // Bonus for method diversity
            var methodCount = completedAttempts.Select(v => v.Method).Distinct().Count();
            var methodBonus = Math.Min(30m, methodCount * 10);

            return Math.Min(100, score + methodBonus);
        }

        private decimal CalculateMarketDemand(List<UserFeedbackSession> feedbackSessions)
        {
            if (!feedbackSessions.Any()) return 50m;

            // Analyze patterns indicating market demand
            var demandIndicators = 0m;

            // High ratings indicate strong demand
            var avgRating = feedbackSessions.SelectMany(s => s.FeedbackData)
                .Average(f => f.Rating.PercentageScore());
            demandIndicators += avgRating * 0.6m;

            // Multiple sessions from different users
            var uniqueParticipants = feedbackSessions.Select(s => s.ParticipantEmail).Distinct().Count();
            var participantBonus = Math.Min(25m, uniqueParticipants * 5);
            demandIndicators += participantBonus;

            // Recency of feedback (recent feedback = active interest)
            var recentSessions = feedbackSessions.Count(s => 
                s.CompletedDate.HasValue && 
                s.CompletedDate.Value >= DateTime.UtcNow.AddDays(-30));
            var recencyBonus = Math.Min(15m, recentSessions * 3);
            demandIndicators += recencyBonus;

            return Math.Min(100, demandIndicators);
        }

        private List<string> IdentifyStrengthAreas(Dictionary<string, decimal> componentScores)
        {
            return componentScores
                .Where(kvp => kvp.Value >= 75)
                .OrderByDescending(kvp => kvp.Value)
                .Select(kvp => kvp.Key)
                .ToList();
        }

        private List<string> IdentifyWeaknessAreas(Dictionary<string, decimal> componentScores)
        {
            return componentScores
                .Where(kvp => kvp.Value < 60)
                .OrderBy(kvp => kvp.Value)
                .Select(kvp => kvp.Key)
                .ToList();
        }

        private decimal CalculateConfidence(ProblemHypothesis hypothesis, List<UserFeedbackSession> feedbackSessions)
        {
            var confidence = 50m; // Base confidence

            // More validation attempts = higher confidence
            var attemptCount = hypothesis.ValidationAttempts.Count;
            confidence += Math.Min(25m, attemptCount * 5);

            // Method diversity increases confidence
            var methods = hypothesis.ValidationAttempts.Select(v => v.Method).Distinct().Count();
            confidence += Math.Min(15m, methods * 5);

            // Recent validation increases confidence
            var recentValidations = hypothesis.ValidationAttempts.Count(v => 
                v.CompletedDate.HasValue && 
                v.CompletedDate.Value >= DateTime.UtcNow.AddDays(-60));
            confidence += Math.Min(10m, recentValidations * 3);

            return Math.Min(100, confidence);
        }

        private List<string> GeneratePortfolioInsights(List<ProblemSolutionFitScore> scores)
        {
            var insights = new List<string>();

            if (scores.Any())
            {
                var avgScore = scores.Average(s => s.OverallScore);
                insights.Add($"Portfolio average fit score: {avgScore:F1}%");

                var strongFit = scores.Count(s => s.OverallScore >= 70);
                var weakFit = scores.Count(s => s.OverallScore < 50);
                
                insights.Add($"{strongFit}/{scores.Count} hypotheses show strong fit (≥70%)");
                if (weakFit > 0)
                    insights.Add($"{weakFit}/{scores.Count} hypotheses need attention (<50%)");

                // Identify common weakness patterns
                var allWeaknesses = scores.SelectMany(s => s.WeaknessAreas).ToList();
                var commonWeakness = allWeaknesses.GroupBy(w => w)
                    .OrderByDescending(g => g.Count())
                    .FirstOrDefault();

                if (commonWeakness != null && commonWeakness.Count() > scores.Count * 0.5)
                    insights.Add($"Common weakness across portfolio: {commonWeakness.Key}");
            }

            return insights;
        }

        private List<string> GeneratePortfolioActions(List<ProblemSolutionFitScore> scores)
        {
            var actions = new List<string>();

            var lowScores = scores.Where(s => s.OverallScore < 60).ToList();
            if (lowScores.Any())
                actions.Add($"Prioritize improvement of {lowScores.Count} low-performing hypotheses");

            var needValidation = scores.Where(s => s.Confidence < 70).ToList();
            if (needValidation.Any())
                actions.Add($"Increase validation efforts for {needValidation.Count} low-confidence hypotheses");

            if (scores.Average(s => s.OverallScore) < 65)
                actions.Add("Consider portfolio-wide review of problem definitions and solution approaches");

            return actions;
        }

        private string CalculateTrend(List<FitEvolutionPoint> points)
        {
            if (points.Count < 2) return "Insufficient Data";

            var firstScore = points.First().Score;
            var lastScore = points.Last().Score;
            var change = lastScore - firstScore;

            return change switch
            {
                > 10 => "Improving",
                < -10 => "Declining",
                _ => "Stable"
            };
        }

        private decimal CalculateTrendStrength(List<FitEvolutionPoint> points)
        {
            if (points.Count < 2) return 0;

            var scores = points.Select(p => (double)p.Score).ToArray();
            var x = Enumerable.Range(0, scores.Length).Select(i => (double)i).ToArray();

            // Simple linear regression to calculate R-squared
            var n = scores.Length;
            var sumX = x.Sum();
            var sumY = scores.Sum();
            var sumXY = x.Zip(scores, (xi, yi) => xi * yi).Sum();
            var sumXX = x.Sum(xi => xi * xi);
            var sumYY = scores.Sum(yi => yi * yi);

            var numerator = n * sumXY - sumX * sumY;
            var denominator = Math.Sqrt((n * sumXX - sumX * sumX) * (n * sumYY - sumY * sumY));

            if (denominator == 0) return 0;

            var correlation = numerator / denominator;
            return (decimal)(correlation * correlation * 100); // R-squared as percentage
        }

        private List<string> IdentifyInfluencingFactors(ProblemHypothesis hypothesis)
        {
            var factors = new List<string>();

            if (hypothesis.ValidationAttempts.Any(v => v.Method.Contains("Interview")))
                factors.Add("Customer interview insights");

            if (hypothesis.RelatedCustomerSegmentId.HasValue)
                factors.Add("Targeted customer segment");

            if (hypothesis.ValidationAttempts.Count(v => v.Status == ValidationStatus.Validated) >= 2)
                factors.Add("Multiple successful validations");

            if (hypothesis.ValidationAttempts.Any(v => v.Metrics.Any()))
                factors.Add("Quantitative validation metrics");

            return factors;
        }

        private List<FitMilestone> IdentifyMilestones(List<FitEvolutionPoint> points)
        {
            var milestones = new List<FitMilestone>();

            for (int i = 1; i < points.Count; i++)
            {
                var current = points[i];
                var previous = points[i - 1];
                var change = current.Score - previous.Score;

                if (Math.Abs(change) >= 20) // Significant change
                {
                    milestones.Add(new FitMilestone
                    {
                        Date = current.Date,
                        Description = current.Event,
                        ScoreChange = change,
                        Impact = change > 0 ? "Positive" : "Negative"
                    });
                }
            }

            return milestones;
        }

        private FitProjection GenerateProjection(List<FitEvolutionPoint> points, string trend)
        {
            if (points.Count < 2) return null;

            var lastPoint = points.Last();
            var projectionDate = DateTime.UtcNow.AddDays(90);

            var projectedScore = trend switch
            {
                "Improving" => Math.Min(100, lastPoint.Score + 15),
                "Declining" => Math.Max(0, lastPoint.Score - 15),
                _ => lastPoint.Score
            };

            var confidence = points.Count >= 5 ? 80m : 60m;

            return new FitProjection
            {
                ProjectionDate = projectionDate,
                ProjectedScore = projectedScore,
                Confidence = confidence,
                Assumptions = new List<string>
                {
                    "Current validation approach continues",
                    "No major market changes",
                    "Consistent effort allocation"
                },
                Scenarios = new List<string>
                {
                    $"Best case: {Math.Min(100, projectedScore + 10)}%",
                    $"Most likely: {projectedScore}%",
                    $"Worst case: {Math.Max(0, projectedScore - 10)}%"
                }
            };
        }

        private FitImprovementRecommendation GenerateComponentRecommendation(string component, decimal score, ProblemHypothesis hypothesis)
        {
            return component switch
            {
                "Problem Clarity" => new FitImprovementRecommendation
                {
                    Area = component,
                    Recommendation = "Conduct additional customer research to better define the problem",
                    PotentialImprovement = 80 - score,
                    Priority = score < 40 ? FeaturePriority.Critical : FeaturePriority.High,
                    ImplementationEffort = 14,
                    ActionSteps = new List<string>
                    {
                        "Conduct customer interviews",
                        "Refine problem statement",
                        "Validate with target segment",
                        "Document findings"
                    },
                    SuccessCriteria = "Clear, validated problem statement"
                },

                "Solution Effectiveness" => new FitImprovementRecommendation
                {
                    Area = component,
                    Recommendation = "Iterate on solution based on user feedback",
                    PotentialImprovement = 75 - score,
                    Priority = FeaturePriority.High,
                    ImplementationEffort = 21,
                    ActionSteps = new List<string>
                    {
                        "Analyze user feedback patterns",
                        "Identify solution gaps",
                        "Implement improvements",
                        "Test with users"
                    },
                    SuccessCriteria = "User satisfaction above 75%"
                },

                "Customer Validation" => new FitImprovementRecommendation
                {
                    Area = component,
                    Recommendation = "Increase validation frequency and method diversity",
                    PotentialImprovement = 70 - score,
                    Priority = FeaturePriority.Medium,
                    ImplementationEffort = 10,
                    ActionSteps = new List<string>
                    {
                        "Plan validation experiments",
                        "Use multiple validation methods",
                        "Set validation metrics",
                        "Track validation progress"
                    },
                    SuccessCriteria = "Consistent validation results across methods"
                },

                "Market Demand" => new FitImprovementRecommendation
                {
                    Area = component,
                    Recommendation = "Gather more market validation data",
                    PotentialImprovement = 65 - score,
                    Priority = FeaturePriority.Medium,
                    ImplementationEffort = 14,
                    ActionSteps = new List<string>
                    {
                        "Expand user testing",
                        "Conduct market surveys",
                        "Analyze competitor landscape",
                        "Validate market size"
                    },
                    SuccessCriteria = "Strong market demand indicators"
                },

                _ => null
            };
        }

        private string CalculatePercentile(decimal score, decimal average, decimal topQuartile, decimal bottomQuartile)
        {
            if (score >= topQuartile) return "Top 25%";
            if (score >= average) return "Above Average";
            if (score >= bottomQuartile) return "Below Average";
            return "Bottom 25%";
        }

        private string GenerateBenchmarkInterpretation(decimal score, string percentile)
        {
            return score switch
            {
                >= 80 => $"Excellent fit score ({percentile}). Strong problem-solution alignment.",
                >= 65 => $"Good fit score ({percentile}). Room for optimization in specific areas.",
                >= 50 => $"Moderate fit score ({percentile}). Significant improvement opportunities exist.",
                _ => $"Poor fit score ({percentile}). Fundamental review of problem-solution fit needed."
            };
        }

        private List<string> GenerateBenchmarkInsights(ProblemSolutionFitScore fitScore, decimal industryAverage)
        {
            var insights = new List<string>();

            var difference = fitScore.OverallScore - industryAverage;
            if (difference > 10)
                insights.Add($"Performing {difference:F1} points above industry average");
            else if (difference < -10)
                insights.Add($"Performing {Math.Abs(difference):F1} points below industry average");

            if (fitScore.StrengthAreas.Any())
                insights.Add($"Strong performance in: {string.Join(", ", fitScore.StrengthAreas)}");

            if (fitScore.WeaknessAreas.Any())
                insights.Add($"Improvement needed in: {string.Join(", ", fitScore.WeaknessAreas)}");

            return insights;
        }

        private List<string> GenerateBenchmarkOpportunities(ProblemSolutionFitScore fitScore, decimal topQuartile)
        {
            var opportunities = new List<string>();

            var gap = topQuartile - fitScore.OverallScore;
            if (gap > 0)
                opportunities.Add($"Opportunity to improve by {gap:F1} points to reach top quartile");

            foreach (var weakness in fitScore.WeaknessAreas)
            {
                opportunities.Add($"Focus on improving {weakness} for competitive advantage");
            }

            if (fitScore.Confidence < 80)
                opportunities.Add("Increase validation efforts to improve confidence in fit assessment");

            return opportunities;
        }

        #endregion
    }
}
