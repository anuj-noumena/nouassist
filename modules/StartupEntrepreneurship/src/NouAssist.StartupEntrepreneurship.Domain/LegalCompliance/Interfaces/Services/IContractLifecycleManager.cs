namespace NouAssist.StartupEntrepreneurship.LegalCompliance.Interfaces.Services
{
    /// <summary>
    /// Service for managing the contract/agreement lifecycle
    /// </summary>
    public interface IContractLifecycleManager
    {
        /// <summary>
        /// Creates a new legal agreement
        /// </summary>
        Task<LegalAgreement> CreateAgreementAsync(
            string title,
            string description,
            LegalAgreementType type,
            string counterpartyName,
            DateTime effectiveDate,
            DateTime? expirationDate,
            string referenceNumber,
            string country,
            string state = null,
            decimal? value = null,
            string currency = null);
        
        /// <summary>
        /// Updates an existing agreement
        /// </summary>
        Task<LegalAgreement> UpdateAgreementAsync(
            Guid id,
            string title,
            string description,
            string counterpartyName,
            string referenceNumber,
            string country,
            string state = null,
            decimal? value = null,
            string currency = null);
        
        /// <summary>
        /// Updates the status of an agreement
        /// </summary>
        Task UpdateAgreementStatusAsync(
            Guid id,
            FilingStatusType newStatus,
            string notes = null);
        
        /// <summary>
        /// Adds a term to an agreement
        /// </summary>
        Task AddAgreementTermAsync(
            Guid agreementId,
            string title,
            string description,
            bool isCritical = false,
            DateTime? deadlineDate = null);
        
        /// <summary>
        /// Adds a reminder to an agreement
        /// </summary>
        Task AddAgreementReminderAsync(
            Guid agreementId,
            string title,
            DateTime reminderDate,
            string description = null,
            int? daysBeforeEvent = null,
            bool isRecurring = false,
            string recurrencePattern = null);
        
        /// <summary>
        /// Renews an agreement
        /// </summary>
        Task<LegalAgreement> RenewAgreementAsync(
            Guid id,
            DateTime newEffectiveDate,
            DateTime? newExpirationDate,
            string notes = null);
        
        /// <summary>
        /// Terminates an agreement
        /// </summary>
        Task TerminateAgreementAsync(
            Guid id,
            DateTime terminationDate,
            string reason);
        
        /// <summary>
        /// Gets agreements that are about to expire
        /// </summary>
        Task<List<LegalAgreement>> GetExpiringAgreementsAsync(int daysThreshold);
        
        /// <summary>
        /// Gets active agreements as of a specific date
        /// </summary>
        Task<List<LegalAgreement>> GetActiveAgreementsAsync(DateTime referenceDate);
    }
}
