namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Repositories
{
    /// <summary>
    /// Repository interface for InvestorProfile aggregate
    /// </summary>
    public interface IInvestorProfileRepository : IRepository<InvestorProfile, Guid>
    {
        Task<InvestorProfile> FindByEmailAsync(string email, CancellationToken cancellationToken = default);
        
        Task<List<InvestorProfile>> GetListAsync(
            Specification<InvestorProfile> specification,
            CancellationToken cancellationToken = default);
        
        Task<int> CountAsync(
            Specification<InvestorProfile> specification,
            CancellationToken cancellationToken = default);
        
        Task<bool> AnyAsync(
            Specification<InvestorProfile> specification,
            CancellationToken cancellationToken = default);
        
        Task<List<InvestorProfile>> GetInvestorsByGroupAsync(
            Guid groupId,
            CancellationToken cancellationToken = default);
        
        Task<List<InvestorProfile>> GetByIdsAsync(
            List<Guid> ids,
            CancellationToken cancellationToken = default);
        
        Task<bool> IsEmailUniqueAsync(
            string email,
            Guid? excludeId = null,
            CancellationToken cancellationToken = default);
    }
}
