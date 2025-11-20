namespace NouAssist.RequirementsManagement.Traceability.Repositories
{
    /// <summary>
    /// Repository interface for managing TraceLink aggregates
    /// </summary>
    public interface ITraceLinkRepository : IRepository<TraceLink, Guid>
    {
        /// <summary>
        /// Finds a trace link by its source and target artifact identifiers and link type
        /// </summary>
        Task<TraceLink> FindByArtifactsAsync(
            string sourceArtifactId, 
            string targetArtifactId, 
            LinkType linkType,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all trace links where the specified artifact is either source or target
        /// </summary>
        Task<List<TraceLink>> GetLinksForArtifactAsync(
            string artifactId, 
            ArtifactType artifactType = ArtifactType.Other,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all suspect links that need verification
        /// </summary>
        Task<List<TraceLink>> GetSuspectLinksAsync(
            int maxResultCount = 50,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if a trace link exists between the specified artifacts with the given link type
        /// </summary>
        Task<bool> ExistsAsync(
            string sourceArtifactId, 
            string targetArtifactId, 
            LinkType linkType,
            CancellationToken cancellationToken = default);
    }
}
