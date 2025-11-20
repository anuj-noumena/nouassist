namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Repositories
{
    /// <summary>
    /// Repository interface for FundraisingRound aggregate
    /// </summary>
    public interface IFundraisingRoundRepository : IRepository<FundraisingRound, Guid>
    {
        Task<FundraisingRound> GetWithDetailsAsync(
            Guid id, 
            CancellationToken cancellationToken = default);
        
        Task<List<FundraisingRound>> GetActiveRoundsAsync(
            CancellationToken cancellationToken = default);
        
        Task<List<FundraisingRound>> GetByStageAsync(
            RoundStage stage, 
            CancellationToken cancellationToken = default);
        
        Task<List<FundraisingRound>> GetByTypeAsync(
            RoundType type, 
            CancellationToken cancellationToken = default);
        
        Task<List<FundraisingRound>> GetBySpecificationAsync(
            FundraisingRoundSpecification specification,
            CancellationToken cancellationToken = default);
    }
}
