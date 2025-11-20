namespace NouAssist.BusinessOperations.FreelanceOperations.Interfaces.Services
{
    /// <summary>
    /// Domain service interface for Upwork integration
    /// </summary>
    public interface IUpworkConnector
    {
        /// <summary>
        /// Sets Upwork API credentials
        /// </summary>
        Task SetCredentialsAsync(UpworkCredentials credentials);
        
        /// <summary>
        /// Gets Upwork API credentials
        /// </summary>
        Task<UpworkCredentials> GetCredentialsAsync();
        
        /// <summary>
        /// Creates an Upwork engagement from job data
        /// </summary>
        Task<UpworkEngagement> ImportJobAsync(
            string upworkJobId,
            string jobTitle,
            string clientName,
            string jobLink,
            string contractType);
        
        /// <summary>
        /// Updates an Upwork engagement with contract data
        /// </summary>
        Task UpdateEngagementWithContractAsync(
            Guid engagementId,
            string upworkContractId,
            DateTime startDate);
        
        /// <summary>
        /// Links an Upwork engagement to internal project and client
        /// </summary>
        Task LinkEngagementToSystemAsync(
            Guid engagementId,
            Guid? projectId,
            Guid? clientId);
        
        /// <summary>
        /// Gets active Upwork engagements
        /// </summary>
        Task<List<UpworkEngagement>> GetActiveEngagementsAsync();
    }
}
