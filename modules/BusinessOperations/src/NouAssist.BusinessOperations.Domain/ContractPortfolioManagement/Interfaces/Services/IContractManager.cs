namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Interfaces.Services
{
    /// <summary>
    /// Defines methods for managing contracts
    /// </summary>
    public interface IContractManager
    {
        /// <summary>
        /// Creates a new contract
        /// </summary>
        Task<ContractAggregate> CreateContractAsync(
            string title,
            string referenceNumber,
            string description,
            DateTime effectiveDate,
            DateTime? expirationDate,
            decimal totalValue,
            string currency,
            ContractTemplateReference templateReference,
            DocumentId documentId,
            Guid? tenantId = null);

        /// <summary>
        /// Creates a new version of a contract
        /// </summary>
        Task<ContractVersion> CreateContractVersionAsync(
            Guid contractId,
            string versionLabel,
            string changeReason,
            DocumentId documentId);

        /// <summary>
        /// Changes the status of a contract
        /// </summary>
        Task<ContractAggregate> ChangeContractStatusAsync(
            Guid contractId,
            ContractStatus newStatus);

        /// <summary>
        /// Adds an amendment to a contract
        /// </summary>
        Task<ContractAmendment> AddContractAmendmentAsync(
            Guid contractId,
            string title,
            string description,
            DateTime effectiveDate,
            List<string> affectedSections,
            DocumentId documentId);

        /// <summary>
        /// Adds an alert to a contract
        /// </summary>
        Task<ContractAlert> AddContractAlertAsync(
            Guid contractId,
            string title,
            string description,
            ContractAlertType alertType,
            DateTime triggerDate,
            int daysInAdvance,
            bool isRecurring = false,
            string frequency = null);

        /// <summary>
        /// Gets contracts expiring within a specified time period
        /// </summary>
        Task<List<ContractAggregate>> GetExpiringContractsAsync(
            int daysThreshold,
            Guid? tenantId = null);

        /// <summary>
        /// Gets contracts needing attention
        /// </summary>
        Task<List<ContractAggregate>> GetContractsNeedingAttentionAsync(
            int expirationDaysThreshold,
            Guid? tenantId = null);
    }
}
