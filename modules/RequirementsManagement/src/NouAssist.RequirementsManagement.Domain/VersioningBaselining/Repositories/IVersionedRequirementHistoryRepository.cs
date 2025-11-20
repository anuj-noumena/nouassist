namespace NouAssist.RequirementsManagement.VersioningBaselining.Repositories
{
    /// <summary>
    /// Repository interface for VersionedRequirementHistory aggregate
    /// </summary>
    public interface IVersionedRequirementHistoryRepository : IRepository<VersionedRequirementHistory, Guid>
    {
        /// <summary>
        /// Gets the version history for a specific requirement
        /// </summary>
        Task<VersionedRequirementHistory> GetByRequirementIdAsync(
            Guid requirementId, 
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Checks if a version history exists for a specific requirement
        /// </summary>
        Task<bool> ExistsByRequirementIdAsync(
            Guid requirementId, 
            CancellationToken cancellationToken = default);
    }
}
