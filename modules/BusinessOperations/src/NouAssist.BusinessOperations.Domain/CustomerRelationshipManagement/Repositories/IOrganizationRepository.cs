namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Repositories
{
    /// <summary>
    /// Repository interface for Organization aggregate
    /// </summary>
    public interface IOrganizationRepository : IRepository<Organization, Guid>
    {
        /// <summary>
        /// Checks if an organization with the specified name already exists
        /// </summary>
        Task<bool> ExistsByNameAsync(
            string name, 
            Guid? excludeId = null, 
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Gets organizations by type
        /// </summary>
        Task<List<Organization>> GetByTypeAsync(
            OrganizationType type, 
            int skipCount = 0, 
            int maxResultCount = 10, 
            string sorting = null, 
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Gets child organizations of the specified parent
        /// </summary>
        Task<List<Organization>> GetChildrenAsync(
            Guid parentId, 
            int skipCount = 0, 
            int maxResultCount = 10, 
            string sorting = null, 
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Gets organizations with the specified tag
        /// </summary>
        Task<List<Organization>> GetByTagAsync(
            string tag, 
            int skipCount = 0, 
            int maxResultCount = 10, 
            string sorting = null, 
            CancellationToken cancellationToken = default);
    }
}
