namespace NouAssist.BusinessOperations.FreelanceOperations.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for UpworkEngagement aggregate
    /// </summary>
    public interface IUpworkEngagementRepository : IRepository<UpworkEngagement, Guid>
    {
        /// <summary>
        /// Gets an engagement by Upwork job ID
        /// </summary>
        Task<UpworkEngagement> GetByUpworkJobIdAsync(
            string upworkJobId,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Gets an engagement by Upwork contract ID
        /// </summary>
        Task<UpworkEngagement> GetByUpworkContractIdAsync(
            string upworkContractId,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Gets list of engagements using the given specification
        /// </summary>
        Task<List<UpworkEngagement>> GetListAsync(
            Specification<UpworkEngagement> specification,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Gets count of engagements using the given specification
        /// </summary>
        Task<int> CountAsync(
            Specification<UpworkEngagement> specification,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Checks if any engagements match the given specification
        /// </summary>
        Task<bool> AnyAsync(
            Specification<UpworkEngagement> specification,
            CancellationToken cancellationToken = default);
    }
}
