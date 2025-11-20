namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Repositories
{
    /// <summary>
    /// Repository for StrategicPivot aggregate
    /// </summary>
    public interface IStrategicPivotRepository : IRepository<StrategicPivot, Guid>
    {
        Task<List<StrategicPivot>> GetByTypeAsync(PivotType type, Guid? tenantId, CancellationToken cancellationToken = default);
        
        Task<List<StrategicPivot>> GetByStatusAsync(PivotStatus status, Guid? tenantId, CancellationToken cancellationToken = default);
        
        Task<List<StrategicPivot>> GetByInitiatorAsync(Guid initiatorId, CancellationToken cancellationToken = default);
        
        Task<List<StrategicPivot>> GetActivePivotsAsync(Guid? tenantId, CancellationToken cancellationToken = default);
        
        Task<List<StrategicPivot>> GetOverduePivotsAsync(DateTime currentDate, Guid? tenantId, CancellationToken cancellationToken = default);
        
        Task<StrategicPivot> GetWithImpactMetricsAsync(Guid id, CancellationToken cancellationToken = default);
        
        Task<List<StrategicPivot>> GetByDateRangeAsync(
            DateTime startDate, 
            DateTime endDate, 
            Guid? tenantId, 
            CancellationToken cancellationToken = default);
        
        Task<List<StrategicPivot>> GetBySpecificationAsync(
            BusinessModelManagementSpecification<StrategicPivot> specification,
            CancellationToken cancellationToken = default);
    }
}
