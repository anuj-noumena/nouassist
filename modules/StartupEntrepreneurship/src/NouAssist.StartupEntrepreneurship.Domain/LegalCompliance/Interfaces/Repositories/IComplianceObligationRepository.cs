namespace NouAssist.StartupEntrepreneurship.LegalCompliance.Interfaces.Repositories
{
    /// <summary>
    /// Repository for compliance obligations
    /// </summary>
    public interface IComplianceObligationRepository : IRepository<ComplianceObligation, Guid>
    {
        /// <summary>
        /// Gets obligations with approaching deadlines
        /// </summary>
        Task<List<ComplianceObligation>> GetWithApproachingDeadlinesAsync(
            int daysThreshold, 
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets obligations by status
        /// </summary>
        Task<List<ComplianceObligation>> GetByStatusAsync(
            ComplianceStatus status, 
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets obligations by jurisdiction
        /// </summary>
        Task<List<ComplianceObligation>> GetByJurisdictionAsync(
            string country,
            string state = null,
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets overdue obligations
        /// </summary>
        Task<List<ComplianceObligation>> GetOverdueObligationsAsync(
            DateTime referenceDate,
            CancellationToken cancellationToken = default);
    }
}
