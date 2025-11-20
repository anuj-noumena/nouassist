namespace NouAssist.RequirementsManagement.VersioningBaselining.Repositories
{
    /// <summary>
    /// Repository interface for RequirementBranch aggregate
    /// </summary>
    public interface IRequirementBranchRepository : IRepository<RequirementBranch, Guid>
    {
        /// <summary>
        /// Checks if a branch with the given name already exists
        /// </summary>
        Task<bool> ExistsByNameAsync(
            string name, 
            Guid? excludeId = null, 
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets a branch by its name
        /// </summary>
        Task<RequirementBranch> GetByNameAsync(
            string name, 
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets a list of branches by status
        /// </summary>
        Task<List<RequirementBranch>> GetListByStatusAsync(
            BranchStatus status, 
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets a list of branches created from a specific source
        /// </summary>
        Task<List<RequirementBranch>> GetListBySourceAsync(
            Guid sourceId, 
            string sourceType, 
            CancellationToken cancellationToken = default);
    }
}
