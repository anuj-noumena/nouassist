namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Repositories
{
    /// <summary>
    /// Repository interface for Prospect aggregate
    /// </summary>
    public interface IProspectRepository : IRepository<Prospect, Guid>
    {
        Task<Prospect> GetWithDetailsAsync(
            Guid id, 
            CancellationToken cancellationToken = default);
        
        Task<List<Prospect>> GetByFundraisingRoundAsync(
            Guid fundraisingRoundId, 
            CancellationToken cancellationToken = default);
        
        Task<List<Prospect>> GetByStatusAsync(
            ProspectStatus status, 
            CancellationToken cancellationToken = default);
        
        Task<int> GetCountByStatusAsync(
            ProspectStatus status, 
            CancellationToken cancellationToken = default);
        
        Task<List<Prospect>> GetHighValueProspectsAsync(
            decimal minimumOpportunityValue = 1000000,
            CancellationToken cancellationToken = default);
        
        Task<List<Prospect>> GetProspectsNeedingFollowUpAsync(
            int maxDaysSinceLastInteraction = 14,
            CancellationToken cancellationToken = default);
        
        Task<List<Prospect>> GetBySpecificationAsync(
            ProspectSpecification specification,
            CancellationToken cancellationToken = default);
    }
}
