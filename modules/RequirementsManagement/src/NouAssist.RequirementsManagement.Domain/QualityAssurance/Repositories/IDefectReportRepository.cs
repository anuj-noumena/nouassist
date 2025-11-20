namespace NouAssist.RequirementsManagement.QualityAssurance.Repositories
{
    /// <summary>
    /// Repository for defect reports
    /// </summary>
    public interface IDefectReportRepository : IRepository<DefectReport, Guid>
    {
        /// <summary>
        /// Gets a defect with all its comments
        /// </summary>
        Task<DefectReport> GetWithDetailsAsync(
            Guid id, 
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets defects for a specific test cycle
        /// </summary>
        Task<List<DefectReport>> GetByTestCycleIdAsync(
            Guid testCycleId, 
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets defects for a specific test plan
        /// </summary>
        Task<List<DefectReport>> GetByTestPlanIdAsync(
            Guid testPlanId, 
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets defects assigned to a specific user
        /// </summary>
        Task<List<DefectReport>> GetByAssigneeIdAsync(
            Guid assigneeId, 
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets defects by status
        /// </summary>
        Task<List<DefectReport>> GetByStatusAsync(
            DefectStatus status, 
            CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Gets defects by severity
        /// </summary>
        Task<List<DefectReport>> GetBySeverityAsync(
            DefectSeverity severity, 
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Gets defects matching a specification
        /// </summary>
        Task<List<DefectReport>> GetListAsync(
            QASpecification<DefectReport> specification,
            CancellationToken cancellationToken = default);
    }
}
