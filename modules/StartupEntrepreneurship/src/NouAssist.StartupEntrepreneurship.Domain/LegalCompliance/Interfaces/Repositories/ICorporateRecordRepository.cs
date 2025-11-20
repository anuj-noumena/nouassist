namespace NouAssist.StartupEntrepreneurship.LegalCompliance.Interfaces.Repositories
{
    /// <summary>
    /// Repository for corporate records
    /// </summary>
    public interface ICorporateRecordRepository : IRepository<CorporateRecord, Guid>
    {
        /// <summary>
        /// Gets corporate records by type
        /// </summary>
        Task<List<CorporateRecord>> GetByTypeAsync(
            CorporateRecordType type,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets corporate records by date range
        /// </summary>
        Task<List<CorporateRecord>> GetByDateRangeAsync(
            DateTime startDate,
            DateTime endDate,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets corporate records by status
        /// </summary>
        Task<List<CorporateRecord>> GetByStatusAsync(
            FilingStatusType status,
            CancellationToken cancellationToken = default);
    }
}
