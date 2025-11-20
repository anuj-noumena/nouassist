namespace NouAssist.RequirementsManagement.AnalysisPrioritization.Services
{
    /// <summary>
    /// Domain service for managing requirement relationships and detecting circular dependencies
    /// </summary>
    public class RelationshipMappingService : DomainService, IRelationshipMappingService
    {
        private readonly IAnalyzedRequirementRepository _analyzedRequirementRepository;
        private readonly ILogger<RelationshipMappingService> _logger;

        public RelationshipMappingService(
            IAnalyzedRequirementRepository analyzedRequirementRepository,
            ILogger<RelationshipMappingService> logger)
        {
            _analyzedRequirementRepository = analyzedRequirementRepository;
            _logger = logger;
        }

        public async Task<bool> ValidateRelationshipAsync(
            Guid sourceRequirementId,
            Guid targetRequirementId,
            RelationshipType relationshipType,
            CancellationToken cancellationToken = default)
        {
            // Check if relationship would create circular dependency
            if (relationshipType == RelationshipType.Dependency)
            {
                var wouldCreateCircular = await WouldCreateCircularDependencyAsync(
                    sourceRequirementId, 
                    targetRequirementId, 
                    cancellationToken);

                if (wouldCreateCircular)
                {
                    _logger.LogWarning(
                        "Circular dependency detected between requirements {SourceId} and {TargetId}",
                        sourceRequirementId,
                        targetRequirementId);
                    return false;
                }
            }

            // Additional validation rules can be added here
            return true;
        }

        public async Task<bool> WouldCreateCircularDependencyAsync(
            Guid sourceRequirementId,
            Guid targetRequirementId,
            CancellationToken cancellationToken = default)
        {
            var visitedNodes = new HashSet<Guid>();
            return await CheckCircularDependencyRecursiveAsync(
                targetRequirementId,
                sourceRequirementId,
                visitedNodes,
                cancellationToken);
        }

        private async Task<bool> CheckCircularDependencyRecursiveAsync(
            Guid currentRequirementId,
            Guid targetRequirementId,
            HashSet<Guid> visitedNodes,
            CancellationToken cancellationToken)
        {
            if (visitedNodes.Contains(currentRequirementId))
            {
                return false; // Already visited, no cycle here
            }

            if (currentRequirementId == targetRequirementId)
            {
                return true; // Found circular dependency
            }

            visitedNodes.Add(currentRequirementId);

            // Get all dependencies of current requirement
            var dependencies = await GetRelatedRequirementsAsync(
                currentRequirementId,
                RelationshipType.Dependency,
                cancellationToken);

            foreach (var dependencyId in dependencies)
            {
                var hasCircular = await CheckCircularDependencyRecursiveAsync(
                    dependencyId,
                    targetRequirementId,
                    visitedNodes,
                    cancellationToken);

                if (hasCircular)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<List<Guid>> GetDependencyChainAsync(
            Guid requirementId,
            CancellationToken cancellationToken = default)
        {
            var dependencyChain = new List<Guid>();
            var visitedNodes = new HashSet<Guid>();

            await BuildDependencyChainAsync(
                requirementId,
                dependencyChain,
                visitedNodes,
                cancellationToken);

            return dependencyChain;
        }

        private async Task BuildDependencyChainAsync(
            Guid requirementId,
            List<Guid> dependencyChain,
            HashSet<Guid> visitedNodes,
            CancellationToken cancellationToken)
        {
            if (visitedNodes.Contains(requirementId))
            {
                return; // Avoid infinite recursion
            }

            dependencyChain.Add(requirementId);
            visitedNodes.Add(requirementId);

            var dependencies = await GetRelatedRequirementsAsync(
                requirementId,
                RelationshipType.Dependency,
                cancellationToken);

            foreach (var dependencyId in dependencies)
            {
                await BuildDependencyChainAsync(
                    dependencyId,
                    dependencyChain,
                    visitedNodes,
                    cancellationToken);
            }
        }

        public async Task<List<Guid>> GetRelatedRequirementsAsync(
            Guid requirementId,
            RelationshipType relationshipType,
            CancellationToken cancellationToken = default)
        {
            var analyzedRequirement = await _analyzedRequirementRepository.FindAsync(
                requirementId,
                cancellationToken: cancellationToken);

            if (analyzedRequirement == null)
            {
                return new List<Guid>();
            }

            return analyzedRequirement.Relationships
                .Where(r => r.RelationshipType == relationshipType)
                .Select(r => r.TargetRequirementId)
                .ToList();
        }
    }
}
