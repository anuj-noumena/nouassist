namespace NouAssist.RequirementsManagement.AnalysisPrioritization.Services
{
    /// <summary>
    /// Domain service for performing impact analysis on requirements
    /// </summary>
    public class ImpactAnalysisEngine : DomainService, IImpactAnalysisEngine
    {
        private readonly IAnalyzedRequirementRepository _analyzedRequirementRepository;
        private readonly IRelationshipMappingService _relationshipMappingService;
        private readonly ILogger<ImpactAnalysisEngine> _logger;

        public ImpactAnalysisEngine(
            IAnalyzedRequirementRepository analyzedRequirementRepository,
            IRelationshipMappingService relationshipMappingService,
            ILogger<ImpactAnalysisEngine> logger)
        {
            _analyzedRequirementRepository = analyzedRequirementRepository;
            _relationshipMappingService = relationshipMappingService;
            _logger = logger;
        }

        public async Task<ImpactAssessmentResult> AnalyzeImpactAsync(
            Guid requirementId,
            CancellationToken cancellationToken = default)
        {
            var analyzedRequirement = await _analyzedRequirementRepository.GetAsync(requirementId, cancellationToken: cancellationToken);

            // Analyze technical impact
            var technicalImpact = await AnalyzeTechnicalImpactAsync(analyzedRequirement, cancellationToken);

            // Analyze business impact
            var businessImpact = await AnalyzeBusinessImpactAsync(analyzedRequirement, cancellationToken);

            // Analyze user impact
            var userImpact = await AnalyzeUserImpactAsync(analyzedRequirement, cancellationToken);

            // Analyze risk impact
            var riskImpact = await AnalyzeRiskImpactAsync(analyzedRequirement, cancellationToken);

            // Generate description
            var description = GenerateImpactDescription(technicalImpact, businessImpact, userImpact, riskImpact);

            var result = new ImpactAssessmentResult(
                technicalImpact,
                businessImpact,
                userImpact,
                riskImpact,
                description);

            _logger.LogInformation(
                "Impact analysis completed for requirement {RequirementId}: Overall Score {Score}",
                requirementId,
                result.OverallScore);

            return result;
        }

        public async Task<List<ImpactAssessmentResult>> AnalyzeBatchImpactAsync(
            List<Guid> requirementIds,
            CancellationToken cancellationToken = default)
        {
            var results = new List<ImpactAssessmentResult>();

            foreach (var requirementId in requirementIds)
            {
                try
                {
                    var result = await AnalyzeImpactAsync(requirementId, cancellationToken);
                    results.Add(result);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, 
                        "Failed to analyze impact for requirement {RequirementId}", 
                        requirementId);
                }
            }

            return results;
        }

        public async Task<ImpactAssessmentResult> AnalyzeChangeImpactAsync(
            Guid requirementId,
            string changeDescription,
            CancellationToken cancellationToken = default)
        {
            // Get the base impact analysis
            var baseImpact = await AnalyzeImpactAsync(requirementId, cancellationToken);

            // Analyze additional impact from the change
            var changeMultiplier = AnalyzeChangeComplexity(changeDescription);

            // Apply change multiplier to each impact area
            var adjustedTechnicalImpact = AdjustImpactLevel(baseImpact.TechnicalImpact, changeMultiplier);
            var adjustedBusinessImpact = AdjustImpactLevel(baseImpact.BusinessImpact, changeMultiplier);
            var adjustedUserImpact = AdjustImpactLevel(baseImpact.UserImpact, changeMultiplier);
            var adjustedRiskImpact = AdjustImpactLevel(baseImpact.RiskImpact, changeMultiplier);

            var description = $"Change Impact Analysis: {changeDescription}\n\n{baseImpact.Description}";

            return new ImpactAssessmentResult(
                adjustedTechnicalImpact,
                adjustedBusinessImpact,
                adjustedUserImpact,
                adjustedRiskImpact,
                description);
        }

        private async Task<ImpactLevel> AnalyzeTechnicalImpactAsync(
            AnalyzedRequirement requirement,
            CancellationToken cancellationToken)
        {
            var complexity = requirement.GetNumericAttributeValue("Complexity") ?? 1;
            var effortAttribute = requirement.GetNumericAttributeValue("Effort") ?? 1;

            // Get related requirements to assess scope
            var dependencies = await _relationshipMappingService.GetRelatedRequirementsAsync(
                requirement.Id,
                RelationshipType.Dependency,
                cancellationToken);

            var relatedRequirements = await _relationshipMappingService.GetRelatedRequirementsAsync(
                requirement.Id,
                RelationshipType.Similar,
                cancellationToken);

            // Calculate technical impact based on complexity, effort, and relationships
            var impactScore = (complexity * 0.4m) + (effortAttribute * 0.3m) + 
                             (dependencies.Count * 0.2m) + (relatedRequirements.Count * 0.1m);

            return impactScore switch
            {
                <= 1 => ImpactLevel.Low,
                <= 2.5m => ImpactLevel.Medium,
                <= 4 => ImpactLevel.High,
                _ => ImpactLevel.Critical
            };
        }

        private async Task<ImpactLevel> AnalyzeBusinessImpactAsync(
            AnalyzedRequirement requirement,
            CancellationToken cancellationToken)
        {
            var businessValue = requirement.GetNumericAttributeValue("Business_Value") ?? 1;
            var priority = requirement.GetNumericAttributeValue("Priority") ?? 1;

            // Get conflicts that might affect business impact
            var conflicts = await _relationshipMappingService.GetRelatedRequirementsAsync(
                requirement.Id,
                RelationshipType.Conflict,
                cancellationToken);

            // Calculate business impact
            var impactScore = (businessValue * 0.5m) + (priority * 0.3m) + (conflicts.Count * 0.2m);

            return impactScore switch
            {
                <= 1 => ImpactLevel.Low,
                <= 2.5m => ImpactLevel.Medium,
                <= 4 => ImpactLevel.High,
                _ => ImpactLevel.Critical
            };
        }

        private async Task<ImpactLevel> AnalyzeUserImpactAsync(
            AnalyzedRequirement requirement,
            CancellationToken cancellationToken)
        {
            var stakeholderImpact = requirement.GetNumericAttributeValue("Stakeholder_Impact") ?? 1;
            
            // Check for user-facing functionality indicators
            var domain = requirement.GetStringAttributeValue("Domain");
            var isUserFacing = domain?.ToLower().Contains("ui") == true || 
                              domain?.ToLower().Contains("user") == true ||
                              domain?.ToLower().Contains("interface") == true;

            var baseScore = stakeholderImpact;
            if (isUserFacing)
            {
                baseScore *= 1.5m; // Increase impact for user-facing features
            }

            return baseScore switch
            {
                <= 1.5m => ImpactLevel.Low,
                <= 3 => ImpactLevel.Medium,
                <= 4.5m => ImpactLevel.High,
                _ => ImpactLevel.Critical
            };
        }

        private async Task<ImpactLevel> AnalyzeRiskImpactAsync(
            AnalyzedRequirement requirement,
            CancellationToken cancellationToken)
        {
            var risk = requirement.GetNumericAttributeValue("Risk") ?? 1;
            var technicalDebt = requirement.GetNumericAttributeValue("Technical_Debt") ?? 1;

            // Get dependency chain length as risk factor
            var dependencyChain = await _relationshipMappingService.GetDependencyChainAsync(
                requirement.Id,
                cancellationToken);

            var chainRisk = dependencyChain.Count > 5 ? 1.5m : 1.0m;

            var riskScore = (risk * 0.5m) + (technicalDebt * 0.3m) + (chainRisk * 0.2m);

            return riskScore switch
            {
                <= 1 => ImpactLevel.Low,
                <= 2.5m => ImpactLevel.Medium,
                <= 4 => ImpactLevel.High,
                _ => ImpactLevel.Critical
            };
        }

        private static string GenerateImpactDescription(
            ImpactLevel technicalImpact,
            ImpactLevel businessImpact,
            ImpactLevel userImpact,
            ImpactLevel riskImpact)
        {
            var description = "Impact Analysis Summary:\n";
            description += $"• Technical Impact: {technicalImpact}\n";
            description += $"• Business Impact: {businessImpact}\n";
            description += $"• User Impact: {userImpact}\n";
            description += $"• Risk Impact: {riskImpact}\n\n";

            // Add recommendations based on impact levels
            var maxImpact = new[] { technicalImpact, businessImpact, userImpact, riskImpact }.Max();
            
            description += maxImpact switch
            {
                ImpactLevel.Critical => "Recommendation: This requirement has critical impact and requires careful planning and extensive testing.",
                ImpactLevel.High => "Recommendation: This requirement has high impact and should be prioritized accordingly.",
                ImpactLevel.Medium => "Recommendation: This requirement has moderate impact and should be planned with adequate resources.",
                ImpactLevel.Low => "Recommendation: This requirement has low impact and can be implemented with standard processes.",
                _ => "Recommendation: Impact assessment requires further analysis."
            };

            return description;
        }

        private static decimal AnalyzeChangeComplexity(string changeDescription)
        {
            // Simple heuristic based on change description keywords
            var complexityKeywords = new Dictionary<string, decimal>
            {
                { "integration", 1.5m },
                { "database", 1.3m },
                { "security", 1.4m },
                { "performance", 1.3m },
                { "architecture", 1.6m },
                { "breaking", 1.7m },
                { "migration", 1.5m },
                { "refactor", 1.4m }
            };

            var multiplier = 1.0m;
            var lowerDescription = changeDescription.ToLower();

            foreach (var keyword in complexityKeywords)
            {
                if (lowerDescription.Contains(keyword.Key))
                {
                    multiplier = Math.Max(multiplier, keyword.Value);
                }
            }

            return multiplier;
        }

        private static ImpactLevel AdjustImpactLevel(ImpactLevel baseLevel, decimal multiplier)
        {
            var adjustedValue = (int)baseLevel * multiplier;
            
            return adjustedValue switch
            {
                <= 1 => ImpactLevel.Low,
                <= 2 => ImpactLevel.Medium,
                <= 3 => ImpactLevel.High,
                _ => ImpactLevel.Critical
            };
        }
    }
}
