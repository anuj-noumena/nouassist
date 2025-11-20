namespace NouAssist.BusinessOperations.FreelanceOperations.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for Upwork API credentials
    /// </summary>
    public interface IUpworkCredentialsRepository
    {
        /// <summary>
        /// Gets the Upwork credentials for a tenant
        /// </summary>
        Task<UpworkCredentials> GetCredentialsAsync(Guid? tenantId);
        
        /// <summary>
        /// Sets or updates the Upwork credentials for a tenant
        /// </summary>
        Task SetCredentialsAsync(UpworkCredentials credentials, Guid? tenantId);
    }
}
