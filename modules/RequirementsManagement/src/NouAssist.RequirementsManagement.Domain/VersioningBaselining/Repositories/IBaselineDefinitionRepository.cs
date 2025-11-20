namespace NouAssist.RequirementsManagement.VersioningBaselining.Repositories
{
    /// <summary>
    /// Repository interface for BaselineDefinition aggregate
    /// </summary>
    public interface IBaselineDefinitionRepository : IRepository<BaselineDefinition, Guid>
    {
        /// <summary>
        /// Checks if a baseline with the given name already exists
        /// </summary>
        Task<bool> ExistsByNameAsync(
            string name, 
            Guid? excludeId = null, 
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets a list of baselines by status
        /// </summary>
        Task<List<BaselineDefinition>> GetListByStatusAsync(
            BaselineStatus status, 
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets a list of baselines that contain a specific requirement version
        /// </summary>
        Task<List<BaselineDefinition>> GetListContainingRequirementAsync(
            Guid requirementId, 
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets a list of baselines that contain a specific requirement with a specific version
        /// </summary>
        Task<List<BaselineDefinition>> GetListContainingRequirementVersionAsync(
            Guid requirementId, 
            string version, 
            CancellationToken cancellationToken = default);
    }
}
