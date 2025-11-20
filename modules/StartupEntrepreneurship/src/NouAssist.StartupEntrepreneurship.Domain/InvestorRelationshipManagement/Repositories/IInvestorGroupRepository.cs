namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Repositories
{
    /// <summary>
    /// Repository interface for InvestorGroup entity
    /// </summary>
    public interface IInvestorGroupRepository : IRepository<InvestorGroup, Guid>
    {
        Task<List<InvestorGroup>> GetActiveGroupsAsync(CancellationToken cancellationToken = default);
        
        Task<List<InvestorGroup>> GetGroupsForInvestorAsync(
            Guid investorId,
            CancellationToken cancellationToken = default);
        
        Task<bool> IsNameUniqueAsync(
            string name,
            Guid? excludeId = null,
            CancellationToken cancellationToken = default);
    }
}
