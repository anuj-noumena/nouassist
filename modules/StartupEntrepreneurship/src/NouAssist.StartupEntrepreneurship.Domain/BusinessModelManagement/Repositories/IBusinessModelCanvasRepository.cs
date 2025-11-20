namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Repositories
{
    /// <summary>
    /// Repository for BusinessModelCanvas aggregate
    /// </summary>
    public interface IBusinessModelCanvasRepository : IRepository<BusinessModelCanvas, Guid>
    {
        Task<BusinessModelCanvas> FindByTitleAsync(string title, Guid? tenantId, CancellationToken cancellationToken = default);
        
        Task<List<BusinessModelCanvas>> GetByOwnerAsync(Guid ownerId, CancellationToken cancellationToken = default);
        
        Task<List<BusinessModelCanvas>> GetWithValidationStatusAsync(
            ValidationStatus status, 
            Guid? tenantId, 
            CancellationToken cancellationToken = default);
        
        Task<BusinessModelCanvas> GetWithVersionsAsync(Guid id, CancellationToken cancellationToken = default);
        
        Task<BusinessModelCanvas> GetWithComponentsAsync(Guid id, CancellationToken cancellationToken = default);
        
        Task<BusinessModelCanvas> GetWithVersionsAndComponentsAsync(Guid id, CancellationToken cancellationToken = default);
        
        Task<List<BusinessModelCanvas>> GetBySpecificationAsync(
            BusinessModelManagementSpecification<BusinessModelCanvas> specification,
            CancellationToken cancellationToken = default);
    }
}
