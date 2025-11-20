namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Repositories
{
    /// <summary>
    /// Repository interface for Lead aggregate
    /// </summary>
    public interface ILeadRepository : IRepository<Lead, Guid>
    {
        /// <summary>
        /// Checks if a lead with the specified email already exists
        /// </summary>
        Task<bool> ExistsByEmailAsync(
            string email, 
            Guid? excludeId = null, 
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Gets a lead by email address
        /// </summary>
        Task<Lead> FindByEmailAsync(
            string email, 
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Gets leads with the specified status
        /// </summary>
        Task<List<Lead>> GetByStatusAsync(
            LeadStatus status, 
            int skipCount = 0, 
            int maxResultCount = 10, 
            string sorting = null, 
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Gets leads with a score greater than or equal to the specified value
        /// </summary>
        Task<List<Lead>> GetByMinimumScoreAsync(
            int minimumScore, 
            int skipCount = 0, 
            int maxResultCount = 10, 
            string sorting = null, 
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Gets leads captured from the specified source
        /// </summary>
        Task<List<Lead>> GetBySourceAsync(
            LeadSource source, 
            int skipCount = 0, 
            int maxResultCount = 10, 
            string sorting = null, 
            CancellationToken cancellationToken = default);
    }
}
