namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Repositories
{
    /// <summary>
    /// Repository interface for TermSheetOffer aggregate
    /// </summary>
    public interface ITermSheetOfferRepository : IRepository<TermSheetOffer, Guid>
    {
        Task<TermSheetOffer> GetWithDetailsAsync(
            Guid id, 
            CancellationToken cancellationToken = default);
        
        Task<List<TermSheetOffer>> GetByProspectAsync(
            Guid prospectId, 
            CancellationToken cancellationToken = default);
        
        Task<List<TermSheetOffer>> GetByFundraisingRoundAsync(
            Guid fundraisingRoundId, 
            CancellationToken cancellationToken = default);
        
        Task<List<TermSheetOffer>> GetByStatusAsync(
            TermSheetStatus status, 
            CancellationToken cancellationToken = default);
        
        Task<List<TermSheetOffer>> GetBySpecificationAsync(
            TermSheetSpecification specification,
            CancellationToken cancellationToken = default);
    }
}
