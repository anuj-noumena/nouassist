namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.Repositories
{
    /// <summary>
    /// Repository for FinancialModel aggregate
    /// </summary>
    public interface IFinancialModelRepository : IRepository<FinancialModel, Guid>
    {
        Task<FinancialModel> FindByNameAsync(string name, Guid? tenantId = null, CancellationToken cancellationToken = default);
        Task<List<FinancialModel>> GetActiveModelsAsync(Guid? tenantId = null, CancellationToken cancellationToken = default);
        Task<List<FinancialModel>> GetByStatusAsync(ModelStatus status, Guid? tenantId = null, CancellationToken cancellationToken = default);
        Task<List<FinancialModel>> GetCalculableModelsAsync(Guid? tenantId = null, CancellationToken cancellationToken = default);
        Task<List<FinancialModel>> GetListAsync(FinancialModelSpecification specification, CancellationToken cancellationToken = default);
        Task<int> CountAsync(FinancialModelSpecification specification, CancellationToken cancellationToken = default);
    }
}
