using System.Threading.Tasks;
using NouAssist.RequirementsManagement.Traceability.Enums;
using NouAssist.RequirementsManagement.Traceability.Models;

namespace NouAssist.RequirementsManagement.Traceability.Services.Interfaces
{
    /// <summary>
    /// Service interface for querying traceability information
    /// </summary>
    public interface ITraceabilityQueryService
    {
        /// <summary>
        /// Generates a traceability matrix between source and target artifact types
        /// </summary>
        Task<TraceabilityMatrix> GenerateMatrixAsync(
            ArtifactType sourceType,
            ArtifactType targetType,
            LinkType? linkType = null);

        /// <summary>
        /// Performs impact analysis for a specific artifact
        /// </summary>
        Task<ImpactAnalysisResult> PerformImpactAnalysisAsync(
            string artifactId,
            ArtifactType artifactType,
            int maxDepth = 2);
            
        /// <summary>
        /// Checks coverage between source and target artifact types
        /// </summary>
        Task<CoverageReport> CheckCoverageAsync(
            ArtifactType sourceType,
            ArtifactType targetType,
            LinkType linkType);
    }
}
