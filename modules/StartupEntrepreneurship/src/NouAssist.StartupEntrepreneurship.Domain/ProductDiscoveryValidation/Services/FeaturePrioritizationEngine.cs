namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Services
{
    /// <summary>
    /// Domain service for feature prioritization logic and algorithms
    /// </summary>
    public class FeaturePrioritizationEngine : DomainService, IFeaturePrioritizationEngine
    {
        private readonly IFeatureDefinitionRepository _featureRepository;
        private readonly IProblemHypothesisRepository _hypothesisRepository;
        private readonly ILogger<FeaturePrioritizationEngine> _logger;

        public FeaturePrioritizationEngine(
            IFeatureDefinitionRepository featureRepository,
            IProblemHypothesisRepository hypothesisRepository,
            ILogger<FeaturePrioritizationEngine> logger)
        {
            _featureRepository = featureRepository;
            _hypothesisRepository = hypothesisRepository;
            _logger = logger;
        }

        public async Task<List<FeaturePriorityResult>> CalculateIcePriorityAsync(List<FeatureDefinition> features)
        {
            try
            {
                var results = new List<FeaturePriorityResult>();

                foreach (var feature in features)
                {
                    var latestScore = feature.GetLatestPriorityScore();
                    if (latestScore != null)
                    {
                        var iceScore = latestScore.Input.CalculatePriorityScore();
                        var recommendedPriority = latestScore.Input.DeterminePriority();

                        var criteriaScores = new Dictionary<string, decimal>
                        {
                            ["Impact"] = latestScore.Input.Impact,
                            ["Confidence"] = latestScore.Input.Confidence,
                            ["Effort"] = latestScore.Input.Effort
                        };

                        results.Add(new FeaturePriorityResult
                        {
                            FeatureId = feature.Id,
                            FeatureName = feature.Name,
                            IceScore = iceScore,
                            RecommendedPriority = recommendedPriority,
                            Rationale = GenerateRationale(latestScore.Input),
                            CriteriaScores = criteriaScores,
                            Rank = 0 // Will be set after sorting
                        });
                    }
                }

                // Sort by ICE score and assign ranks
                results = results.OrderByDescending(r => r.IceScore).ToList();
                for (int i = 0; i < results.Count; i++)
                {
                    results[i].Rank = i + 1;
                }

                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to calculate ICE priority for features");
                throw;
            }
        }

        public async Task<PrioritizationMatrix> GeneratePrioritizationMatrixAsync(
            List<FeatureDefinition> features, 
            PrioritizationCriteria criteria)
        {
            try
            {
                var priorityResults = await CalculateWeightedPriorityAsync(features, criteria);

                // Create 2x2 matrix (Impact vs Effort)
                var quadrants = new Dictionary<string, List<FeaturePriorityResult>>
                {
                    ["High Impact, Low Effort"] = new List<FeaturePriorityResult>(),
                    ["High Impact, High Effort"] = new List<FeaturePriorityResult>(),
                    ["Low Impact, Low Effort"] = new List<FeaturePriorityResult>(),
                    ["Low Impact, High Effort"] = new List<FeaturePriorityResult>()
                };

                foreach (var result in priorityResults)
                {
                    var impact = result.CriteriaScores["Impact"];
                    var effort = result.CriteriaScores["Effort"];

                    var quadrant = (impact >= 3, effort <= 3) switch
                    {
                        (true, true) => "High Impact, Low Effort",
                        (true, false) => "High Impact, High Effort",
                        (false, true) => "Low Impact, Low Effort",
                        (false, false) => "Low Impact, High Effort"
                    };

                    quadrants[quadrant].Add(result);
                }

                // Create matrix representation
                var matrix = new List<List<FeaturePriorityResult>>
                {
                    new List<FeaturePriorityResult> { quadrants["High Impact, Low Effort"].OrderByDescending(r => r.IceScore).Take(5).ToList() },
                    new List<FeaturePriorityResult> { quadrants["High Impact, High Effort"].OrderByDescending(r => r.IceScore).Take(5).ToList() },
                    new List<FeaturePriorityResult> { quadrants["Low Impact, Low Effort"].OrderByDescending(r => r.IceScore).Take(5).ToList() },
                    new List<FeaturePriorityResult> { quadrants["Low Impact, High Effort"].OrderByDescending(r => r.IceScore).Take(5).ToList() }
                };

                return new PrioritizationMatrix
                {
                    Matrix = matrix,
                    XAxisLabels = new List<string> { "Low Effort", "High Effort" },
                    YAxisLabels = new List<string> { "High Impact", "Low Impact" },
                    XAxisTitle = "Development Effort",
                    YAxisTitle = "Business Impact",
                    Quadrants = quadrants
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate prioritization matrix");
                throw;
            }
        }

        public async Task<MvpRecommendation> RecommendMvpFeaturesAsync(
            List<FeatureDefinition> features, 
            MvpConstraints constraints)
        {
            try
            {
                var priorityResults = await CalculateIcePriorityAsync(features);

                // Filter features based on constraints
                var eligibleFeatures = priorityResults.Where(f => 
                    !constraints.ExcludedFeatures.Contains(f.FeatureId) &&
                    f.IceScore >= constraints.MinimumIceScore).ToList();

                // Always include must-have features
                var mustHaveFeatures = eligibleFeatures.Where(f => 
                    constraints.MustHaveFeatures.Contains(f.FeatureId)).ToList();

                // Calculate remaining capacity
                var remainingFeatureCount = constraints.MaxFeatureCount - mustHaveFeatures.Count;
                var usedEffort = CalculateTotalEffort(mustHaveFeatures, features);
                var remainingEffort = constraints.MaxDevelopmentEffort - usedEffort;

                // Greedily select additional features
                var additionalFeatures = new List<FeaturePriorityResult>();
                var sortedCandidates = eligibleFeatures
                    .Except(mustHaveFeatures)
                    .OrderByDescending(f => f.IceScore)
                    .ToList();

                foreach (var candidate in sortedCandidates)
                {
                    if (additionalFeatures.Count >= remainingFeatureCount) break;

                    var featureEffort = GetFeatureEffort(candidate.FeatureId, features);
                    if (featureEffort <= remainingEffort)
                    {
                        additionalFeatures.Add(candidate);
                        remainingEffort -= featureEffort;
                    }
                }

                var recommendedFeatures = mustHaveFeatures.Concat(additionalFeatures).ToList();
                var alternatives = sortedCandidates.Except(additionalFeatures).Take(5).ToList();

                var totalEffort = CalculateTotalEffort(recommendedFeatures, features);
                var estimatedDelivery = EstimateDeliveryDate(totalEffort, constraints.TargetLaunchDate);

                var risks = IdentifyMvpRisks(recommendedFeatures, constraints);
                var assumptions = GenerateMvpAssumptions(recommendedFeatures);
                var confidence = CalculateConfidence(recommendedFeatures, constraints);

                return new MvpRecommendation
                {
                    RecommendedFeatures = recommendedFeatures,
                    AlternativeFeatures = alternatives,
                    TotalEffortEstimate = totalEffort,
                    EstimatedDeliveryDate = estimatedDelivery,
                    Rationale = GenerateMvpRationale(recommendedFeatures, constraints),
                    Risks = risks,
                    Assumptions = assumptions,
                    ConfidenceLevel = confidence
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to recommend MVP features");
                throw;
            }
        }

        public async Task<FeatureDependencyAnalysis> AnalyzeFeatureDependenciesAsync(List<FeatureDefinition> features)
        {
            try
            {
                // Simplified dependency analysis based on related hypotheses and business logic
                var dependencies = new Dictionary<Guid, List<Guid>>();
                var coreFeatures = new List<Guid>();
                var optionalFeatures = new List<Guid>();

                // Group features by hypothesis to identify dependencies
                var featuresByHypothesis = features
                    .Where(f => f.RelatedHypothesisId.HasValue)
                    .GroupBy(f => f.RelatedHypothesisId.Value)
                    .ToList();

                foreach (var hypothesisGroup in featuresByHypothesis)
                {
                    var hypothesisFeatures = hypothesisGroup.OrderBy(f => f.Priority).ToList();
                    
                    // First feature is core, others depend on it
                    if (hypothesisFeatures.Any())
                    {
                        var coreFeature = hypothesisFeatures.First();
                        coreFeatures.Add(coreFeature.Id);

                        foreach (var dependentFeature in hypothesisFeatures.Skip(1))
                        {
                            if (!dependencies.ContainsKey(dependentFeature.Id))
                                dependencies[dependentFeature.Id] = new List<Guid>();
                            dependencies[dependentFeature.Id].Add(coreFeature.Id);
                            optionalFeatures.Add(dependentFeature.Id);
                        }
                    }
                }

                // Features without hypothesis are considered optional
                var independentFeatures = features
                    .Where(f => !f.RelatedHypothesisId.HasValue)
                    .Select(f => f.Id)
                    .ToList();
                optionalFeatures.AddRange(independentFeatures);

                var dependencyChains = BuildDependencyChains(dependencies, features);
                var dependencyLevels = CalculateDependencyLevels(dependencies);

                return new FeatureDependencyAnalysis
                {
                    Dependencies = dependencies,
                    DependencyChains = dependencyChains,
                    CoreFeatures = coreFeatures.Distinct().ToList(),
                    OptionalFeatures = optionalFeatures.Distinct().ToList(),
                    CircularDependencies = new List<string>(), // Simplified - no circular dependencies
                    DependencyLevels = dependencyLevels
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to analyze feature dependencies");
                throw;
            }
        }

        public async Task<List<PrioritizationRecommendation>> GenerateRecommendationsAsync(List<FeatureDefinition> features)
        {
            try
            {
                var recommendations = new List<PrioritizationRecommendation>();

                // Check for features without priority scores
                var featuresWithoutScores = features.Where(f => !f.PriorityScores.Any()).ToList();
                if (featuresWithoutScores.Any())
                {
                    recommendations.Add(new PrioritizationRecommendation
                    {
                        Type = "Missing Priority Scores",
                        Description = $"{featuresWithoutScores.Count} features lack priority scoring",
                        AffectedFeatures = featuresWithoutScores.Select(f => f.Id).ToList(),
                        Impact = "Cannot properly prioritize features without scores",
                        Priority = FeaturePriority.High,
                        ActionItems = new List<string> { "Conduct ICE scoring sessions", "Gather stakeholder input" }
                    });
                }

                // Check for outdated priority scores
                var outdatedFeatures = features.Where(f => 
                    f.PriorityScores.Any() && 
                    f.GetLatestPriorityScore().CalculatedAt < DateTime.UtcNow.AddDays(-90)).ToList();

                if (outdatedFeatures.Any())
                {
                    recommendations.Add(new PrioritizationRecommendation
                    {
                        Type = "Outdated Priority Scores",
                        Description = $"{outdatedFeatures.Count} features have outdated priority scores",
                        AffectedFeatures = outdatedFeatures.Select(f => f.Id).ToList(),
                        Impact = "Priorities may not reflect current business needs",
                        Priority = FeaturePriority.Medium,
                        ActionItems = new List<string> { "Review and update priority scores", "Validate business assumptions" }
                    });
                }

                // Check MVP balance
                var mvpFeatures = features.Where(f => f.IsIncludedInMvp()).ToList();
                var totalFeatures = features.Count;
                var mvpPercentage = totalFeatures > 0 ? (decimal)mvpFeatures.Count / totalFeatures * 100 : 0;

                if (mvpPercentage > 70)
                {
                    recommendations.Add(new PrioritizationRecommendation
                    {
                        Type = "MVP Scope Too Large",
                        Description = $"MVP includes {mvpPercentage:F1}% of all features",
                        AffectedFeatures = mvpFeatures.Select(f => f.Id).ToList(),
                        Impact = "Large MVP scope increases risk and time to market",
                        Priority = FeaturePriority.High,
                        ActionItems = new List<string> { "Reassess MVP scope", "Focus on core value proposition" }
                    });
                }
                else if (mvpPercentage < 20 && totalFeatures > 5)
                {
                    recommendations.Add(new PrioritizationRecommendation
                    {
                        Type = "MVP Scope Too Small",
                        Description = $"MVP includes only {mvpPercentage:F1}% of features",
                        AffectedFeatures = mvpFeatures.Select(f => f.Id).ToList(),
                        Impact = "Small MVP may not provide sufficient value",
                        Priority = FeaturePriority.Medium,
                        ActionItems = new List<string> { "Consider adding high-impact features", "Validate MVP value proposition" }
                    });
                }

                // Check for unvalidated high-priority features
                var unvalidatedHighPriority = features.Where(f => 
                    f.Priority >= FeaturePriority.High && 
                    !f.RelatedHypothesisId.HasValue).ToList();

                if (unvalidatedHighPriority.Any())
                {
                    recommendations.Add(new PrioritizationRecommendation
                    {
                        Type = "Unvalidated High-Priority Features",
                        Description = $"{unvalidatedHighPriority.Count} high-priority features lack hypothesis validation",
                        AffectedFeatures = unvalidatedHighPriority.Select(f => f.Id).ToList(),
                        Impact = "Building unvalidated features increases product risk",
                        Priority = FeaturePriority.High,
                        ActionItems = new List<string> { "Create validation hypotheses", "Conduct user research" }
                    });
                }

                return recommendations.OrderByDescending(r => r.Priority).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate prioritization recommendations");
                throw;
            }
        }

        #region Private Methods

        private async Task<List<FeaturePriorityResult>> CalculateWeightedPriorityAsync(
            List<FeatureDefinition> features, 
            PrioritizationCriteria criteria)
        {
            var results = new List<FeaturePriorityResult>();

            foreach (var feature in features)
            {
                var latestScore = feature.GetLatestPriorityScore();
                if (latestScore != null)
                {
                    var input = latestScore.Input;
                    
                    // Apply weights to criteria
                    var weightedScore = (input.Impact * criteria.ImpactWeight * input.Confidence * criteria.ConfidenceWeight) 
                                       / (input.Effort * criteria.EffortWeight);

                    var criteriaScores = new Dictionary<string, decimal>
                    {
                        ["Impact"] = input.Impact,
                        ["Confidence"] = input.Confidence,
                        ["Effort"] = input.Effort,
                        ["Weighted Score"] = weightedScore
                    };

                    results.Add(new FeaturePriorityResult
                    {
                        FeatureId = feature.Id,
                        FeatureName = feature.Name,
                        IceScore = weightedScore,
                        RecommendedPriority = DeterminePriorityFromScore(weightedScore),
                        Rationale = GenerateWeightedRationale(input, criteria),
                        CriteriaScores = criteriaScores,
                        Rank = 0
                    });
                }
            }

            return results.OrderByDescending(r => r.IceScore).ToList();
        }

        private string GenerateRationale(PriorityInput input)
        {
            var rationale = $"Impact: {input.Impact}/5, Confidence: {input.Confidence}/5, Effort: {input.Effort}/5";
            
            if (!string.IsNullOrWhiteSpace(input.Rationale))
                rationale += $". {input.Rationale}";

            return rationale;
        }

        private string GenerateWeightedRationale(PriorityInput input, PrioritizationCriteria criteria)
        {
            var rationale = GenerateRationale(input);
            
            if (criteria.ImpactWeight != 1.0m || criteria.ConfidenceWeight != 1.0m || criteria.EffortWeight != 1.0m)
            {
                rationale += $" (Weighted: Impact×{criteria.ImpactWeight}, Confidence×{criteria.ConfidenceWeight}, Effort×{criteria.EffortWeight})";
            }

            return rationale;
        }

        private FeaturePriority DeterminePriorityFromScore(decimal score)
        {
            return score switch
            {
                >= 15 => FeaturePriority.Critical,
                >= 10 => FeaturePriority.High,
                >= 5 => FeaturePriority.Medium,
                _ => FeaturePriority.Low
            };
        }

        private decimal CalculateTotalEffort(List<FeaturePriorityResult> priorityResults, List<FeatureDefinition> features)
        {
            return priorityResults.Sum(r => GetFeatureEffort(r.FeatureId, features));
        }

        private decimal GetFeatureEffort(Guid featureId, List<FeatureDefinition> features)
        {
            var feature = features.FirstOrDefault(f => f.Id == featureId);
            var latestScore = feature?.GetLatestPriorityScore();
            return latestScore?.Input.Effort ?? 3; // Default to medium effort
        }

        private DateTime EstimateDeliveryDate(decimal totalEffort, DateTime targetLaunchDate)
        {
            // Simple estimation: each effort point = 1 week
            var estimatedWeeks = totalEffort;
            var estimatedDate = DateTime.UtcNow.AddDays((double)estimatedWeeks * 7);
            
            return estimatedDate > targetLaunchDate ? estimatedDate : targetLaunchDate;
        }

        private List<string> IdentifyMvpRisks(List<FeaturePriorityResult> features, MvpConstraints constraints)
        {
            var risks = new List<string>();

            if (features.Count > constraints.MaxFeatureCount)
                risks.Add("MVP scope exceeds planned feature count");

            var lowConfidenceFeatures = features.Count(f => f.CriteriaScores.GetValueOrDefault("Confidence", 5) < 3);
            if (lowConfidenceFeatures > features.Count * 0.3)
                risks.Add("High number of low-confidence features");

            var highEffortFeatures = features.Count(f => f.CriteriaScores.GetValueOrDefault("Effort", 3) >= 4);
            if (highEffortFeatures > features.Count * 0.5)
                risks.Add("Many features require high development effort");

            return risks;
        }

        private List<string> GenerateMvpAssumptions(List<FeaturePriorityResult> features)
        {
            return new List<string>
            {
                "User needs align with feature priorities",
                "Development estimates are accurate",
                "No major technical blockers will emerge",
                "Market conditions remain stable",
                $"Core features ({features.Take(3).Count()}) will provide sufficient value"
            };
        }

        private decimal CalculateConfidence(List<FeaturePriorityResult> features, MvpConstraints constraints)
        {
            if (!features.Any()) return 0;

            var avgConfidence = features.Average(f => f.CriteriaScores.GetValueOrDefault("Confidence", 3));
            var scopeConfidence = features.Count <= constraints.MaxFeatureCount ? 1.0m : 0.7m;
            
            return (avgConfidence / 5 + scopeConfidence) / 2 * 100;
        }

        private string GenerateMvpRationale(List<FeaturePriorityResult> features, MvpConstraints constraints)
        {
            var mustHaveCount = features.Count(f => constraints.MustHaveFeatures.Contains(f.FeatureId));
            var highPriorityCount = features.Count(f => f.RecommendedPriority >= FeaturePriority.High);
            
            return $"Selected {features.Count} features including {mustHaveCount} must-have and {highPriorityCount} high-priority features. " +
                   $"Prioritized by ICE score with emphasis on validated hypotheses and user value.";
        }

        private List<FeatureDependencyChain> BuildDependencyChains(Dictionary<Guid, List<Guid>> dependencies, List<FeatureDefinition> features)
        {
            var chains = new List<FeatureDependencyChain>();

            foreach (var dependency in dependencies)
            {
                var chain = new List<Guid> { dependency.Key };
                var current = dependency.Key;

                // Follow the dependency chain
                while (dependencies.ContainsKey(current) && dependencies[current].Any())
                {
                    var parent = dependencies[current].First(); // Simplified - take first dependency
                    if (chain.Contains(parent)) break; // Avoid infinite loops
                    
                    chain.Add(parent);
                    current = parent;
                }

                if (chain.Count > 1)
                {
                    var featureNames = chain.Select(id => features.FirstOrDefault(f => f.Id == id)?.Name ?? "Unknown").ToList();
                    
                    chains.Add(new FeatureDependencyChain
                    {
                        FeatureIds = chain,
                        FeatureNames = featureNames,
                        ChainLength = chain.Count,
                        IsCriticalPath = chain.Count >= 3
                    });
                }
            }

            return chains.OrderByDescending(c => c.ChainLength).ToList();
        }

        private Dictionary<Guid, int> CalculateDependencyLevels(Dictionary<Guid, List<Guid>> dependencies)
        {
            var levels = new Dictionary<Guid, int>();

            // Initialize all features at level 0
            foreach (var feature in dependencies.Keys.Concat(dependencies.Values.SelectMany(v => v)).Distinct())
            {
                levels[feature] = 0;
            }

            // Calculate levels iteratively
            bool changed = true;
            while (changed)
            {
                changed = false;
                foreach (var dependency in dependencies)
                {
                    var maxParentLevel = dependency.Value.Max(parent => levels.GetValueOrDefault(parent, 0));
                    var newLevel = maxParentLevel + 1;
                    
                    if (newLevel > levels[dependency.Key])
                    {
                        levels[dependency.Key] = newLevel;
                        changed = true;
                    }
                }
            }

            return levels;
        }

        #endregion
    }
}
