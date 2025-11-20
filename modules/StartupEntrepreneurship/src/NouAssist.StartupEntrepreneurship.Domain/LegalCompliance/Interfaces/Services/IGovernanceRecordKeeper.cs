namespace NouAssist.StartupEntrepreneurship.LegalCompliance.Interfaces.Services
{
    /// <summary>
    /// Service for managing corporate governance records
    /// </summary>
    public interface IGovernanceRecordKeeper
    {
        /// <summary>
        /// Creates a new corporate record
        /// </summary>
        Task<CorporateRecord> CreateCorporateRecordAsync(
            string title,
            string description,
            CorporateRecordType type,
            DateTime recordDate,
            string referenceNumber = null);
        
        /// <summary>
        /// Updates an existing corporate record
        /// </summary>
        Task<CorporateRecord> UpdateCorporateRecordAsync(
            Guid id,
            string title,
            string description,
            DateTime recordDate,
            string referenceNumber = null);
        
        /// <summary>
        /// Updates the status of a corporate record
        /// </summary>
        Task UpdateCorporateRecordStatusAsync(
            Guid id,
            FilingStatusType newStatus,
            string notes = null);
        
        /// <summary>
        /// Gets corporate records by type
        /// </summary>
        Task<List<CorporateRecord>> GetCorporateRecordsByTypeAsync(CorporateRecordType type);
        
        /// <summary>
        /// Gets corporate records within a date range
        /// </summary>
        Task<List<CorporateRecord>> GetCorporateRecordsByDateRangeAsync(
            DateTime startDate,
            DateTime endDate);
    }
}
