namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Interfaces.Services
{
    /// <summary>
    /// Defines methods for monitoring contract status
    /// </summary>
    public interface IContractStatusMonitor
    {
        /// <summary>
        /// Processes alerts for contracts
        /// </summary>
        Task ProcessAlertsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates contract statuses based on effective and expiration dates
        /// </summary>
        Task UpdateContractStatusesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets alerts due for processing
        /// </summary>
        Task<List<ContractAlert>> GetDueAlertsAsync(Guid? tenantId = null);

        /// <summary>
        /// Gets contracts that have expired but are still active
        /// </summary>
        Task<List<ContractAggregate>> GetExpiredContractsAsync(Guid? tenantId = null);
    }
}
