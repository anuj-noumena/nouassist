namespace NouAssist.RequirementsManagement.Traceability.Services
{
    /// <summary>
    /// Service for querying traceability information
    /// </summary>
    public class TraceabilityQueryService : DomainService, ITraceabilityQueryService
    {
        private readonly ITraceLinkRepository _traceLinkRepository;
        private readonly ILogger<TraceabilityQueryService> _logger;

        public TraceabilityQueryService(
            ITraceLinkRepository traceLinkRepository,
            ILogger<TraceabilityQueryService> logger)
        {
            _traceLinkRepository = traceLinkRepository;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<TraceabilityMatrix> GenerateMatrixAsync(
            ArtifactType sourceType,
            ArtifactType targetType,
            LinkType? linkType = null)
        {
            // Create specifications for the query
            var typeSpec = new ArtifactTypeLinksSpecification(sourceType, targetType);
            
            // Add link type specification if provided
            Specification<TraceLink> combinedSpec = typeSpec;
            if (linkType.HasValue)
            {
                var linkTypeSpec = new LinkTypeSpecification(linkType.Value);
                combinedSpec = typeSpec.And(linkTypeSpec);
            }
            
            // Get links that match the specifications
            var links = await _traceLinkRepository.GetListAsync(combinedSpec.ToExpression());
            
            // Build the matrix
            var matrix = new TraceabilityMatrix
            {
                SourceType = sourceType,
                TargetType = targetType,
                LinkType = linkType
            };
            
            // Process links to build the matrix
            foreach (var link in links)
            {
                var sourceId = link.SourceArtifact.ArtifactIdentifier.Id;
                var targetId = link.TargetArtifact.ArtifactIdentifier.Id;
                
                // Add source and target artifacts to the matrices
                if (!matrix.SourceArtifacts.Contains(sourceId))
                {
                    matrix.SourceArtifacts.Add(sourceId);
                }
                
                if (!matrix.TargetArtifacts.Contains(targetId))
                {
                    matrix.TargetArtifacts.Add(targetId);
                }
                
                // Add the link to the matrix
                matrix.AddLink(sourceId, targetId, link.LinkType, link.Id);
            }
            
            _logger.LogInformation(
                "Generated traceability matrix for {SourceType} to {TargetType} with {LinkCount} links",
                sourceType,
                targetType,
                links.Count);
                
            return matrix;
        }

        /// <inheritdoc />
        public async Task<ImpactAnalysisResult> PerformImpactAnalysisAsync(
            string artifactId,
            ArtifactType artifactType,
            int maxDepth = 2)
        {
            var result = new ImpactAnalysisResult
            {
                SourceArtifactId = artifactId,
                SourceArtifactType = artifactType
            };
            
            // Get initial links for the artifact
            var artifactSpec = new ArtifactInvolvementSpecification(artifactId);
            var initialLinks = await _traceLinkRepository.GetListAsync(artifactSpec.ToExpression());
            
            // Perform recursive impact analysis up to the max depth
            await AnalyzeImpactRecursivelyAsync(
                artifactId,
                artifactType,
                initialLinks,
                result,
                0,
                maxDepth,
                new HashSet<string>());
                
            _logger.LogInformation(
                "Performed impact analysis for artifact {ArtifactId} of type {ArtifactType} with {ImpactedCount} impacted artifacts",
                artifactId,
                artifactType,
                result.ImpactedArtifacts.Count);
                
            return result;
        }

        /// <inheritdoc />
        public async Task<CoverageReport> CheckCoverageAsync(
            ArtifactType sourceType,
            ArtifactType targetType,
            LinkType linkType)
        {
            // Generate a matrix to analyze coverage
            var matrix = await GenerateMatrixAsync(sourceType, targetType, linkType);
            
            // Calculate coverage statistics
            var report = new CoverageReport
            {
                SourceType = sourceType,
                TargetType = targetType,
                LinkType = linkType,
                TotalSourceArtifacts = matrix.SourceArtifacts.Count,
                TotalTargetArtifacts = matrix.TargetArtifacts.Count,
                CoveredSourceArtifacts = matrix.Links.Keys.Count,
                CoveredTargetArtifacts = matrix.Links.Values.SelectMany(v => v.Keys).Distinct().Count()
            };
            
            // Calculate coverage percentages
            report.CalculateCoveragePercentages();
            
            // Find uncovered artifacts
            report.UncoveredSourceArtifacts = matrix.SourceArtifacts
                .Except(matrix.Links.Keys)
                .ToList();
                
            report.UncoveredTargetArtifacts = matrix.TargetArtifacts
                .Except(matrix.Links.Values.SelectMany(v => v.Keys).Distinct())
                .ToList();
                
            _logger.LogInformation(
                "Generated coverage report for {SourceType} to {TargetType} with source coverage {SourceCoverage}% and target coverage {TargetCoverage}%",
                sourceType,
                targetType,
                report.SourceCoveragePercentage,
                report.TargetCoveragePercentage);
                
            return report;
        }

        /// <summary>
        /// Recursive helper method to analyze impact of changes to an artifact
        /// </summary>
        private async Task AnalyzeImpactRecursivelyAsync(
            string artifactId,
            ArtifactType artifactType,
            List<TraceLink> currentLevelLinks,
            ImpactAnalysisResult result,
            int currentDepth,
            int maxDepth,
            HashSet<string> processedArtifacts)
        {
            // Stop recursion if we've reached the maximum depth
            if (currentDepth >= maxDepth)
            {
                return;
            }
            
            // Add the current artifact to processed list to avoid cycles
            processedArtifacts.Add($"{artifactType}:{artifactId}");
            
            // Process each link at the current level
            foreach (var link in currentLevelLinks)
            {
                // Determine the "other" artifact in the link
                string nextArtifactId;
                ArtifactType nextArtifactType;
                LinkDirection effectiveDirection;
                
                if (link.SourceArtifact.ArtifactIdentifier.Id == artifactId &&
                    link.SourceArtifact.ArtifactIdentifier.Type == artifactType)
                {
                    // The current artifact is the source, so we're looking at impact on the target
                    nextArtifactId = link.TargetArtifact.ArtifactIdentifier.Id;
                    nextArtifactType = link.TargetArtifact.ArtifactIdentifier.Type;
                    effectiveDirection = LinkDirection.Forward;
                }
                else
                {
                    // The current artifact is the target, so we're looking at impact on the source
                    nextArtifactId = link.SourceArtifact.ArtifactIdentifier.Id;
                    nextArtifactType = link.SourceArtifact.ArtifactIdentifier.Type;
                    effectiveDirection = LinkDirection.Backward;
                }
                
                // Don't follow links that shouldn't propagate impact based on direction
                if ((effectiveDirection == LinkDirection.Forward && 
                     (link.LinkDirection == LinkDirection.Forward || link.LinkDirection == LinkDirection.Bidirectional)) ||
                    (effectiveDirection == LinkDirection.Backward && 
                     (link.LinkDirection == LinkDirection.Backward || link.LinkDirection == LinkDirection.Bidirectional)))
                {
                    // Add the impacted artifact to the result
                    result.AddImpactedArtifact(
                        nextArtifactId,
                        nextArtifactType,
                        link.LinkType,
                        effectiveDirection,
                        currentDepth + 1);
                    
                    // Continue recursion if we haven't processed this artifact already
                    if (!processedArtifacts.Contains($"{nextArtifactType}:{nextArtifactId}"))
                    {
                        // Get links for the next artifact
                        var nextSpec = new ArtifactInvolvementSpecification(nextArtifactId);
                        var nextLinks = await _traceLinkRepository.GetListAsync(nextSpec.ToExpression());
                        
                        // Recursive call to analyze impact
                        await AnalyzeImpactRecursivelyAsync(
                            nextArtifactId,
                            nextArtifactType,
                            nextLinks,
                            result,
                            currentDepth + 1,
                            maxDepth,
                            processedArtifacts);
                    }
                }
            }
        }
    }
}
