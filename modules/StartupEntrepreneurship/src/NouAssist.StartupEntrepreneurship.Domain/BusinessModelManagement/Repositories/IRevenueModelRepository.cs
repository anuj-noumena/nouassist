namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Repositories
{
    /// <summary>
    /// Repository for RevenueModel aggregate
    /// </summary>
    public interface IRevenueModelRepository : IRepository<RevenueModel, Guid>
    {
        Task<RevenueModel> FindByNameAsync(string name, Guid? tenantId, CancellationToken cancellationToken = default);
        
        Task<List<RevenueModel>> GetByOwnerAsync(Guid ownerId, CancellationToken cancellationToken = default);
        
        Task<List<RevenueModel>> GetByTypeAsync(RevenueModelType type, Guid? tenantId, CancellationToken cancellationToken = default);
        
        Task<List<RevenueModel>> GetActiveModelsAsync(Guid? tenantId, CancellationToken cancellationToken = default);
        
        Task<RevenueModel> GetWithPricingModelsAsync(Guid id, CancellationToken cancellationToken = default);
        
        Task<RevenueModel> GetWithScenariosAsync(Guid id, CancellationToken cancellationToken = default);
        
        Task<RevenueModel> GetWithGtmStrategiesAsync(Guid id, CancellationToken cancellationToken = default);
        
        Task<RevenueModel> GetCompleteAsync(Guid id, CancellationToken cancellationToken = default);
        
        Task<List<RevenueModel>> GetBySpecificationAsync(
            BusinessModelManagementSpecification<RevenueModel> specification,
            CancellationToken cancellationToken = default);
    }
}
