namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Services.Interfaces
{
    /// <summary>
    /// Interface for service that compares term sheets
    /// </summary>
    public interface ITermSheetComparisonService
    {
        Task<TermSheetComparisonResult> CompareTermSheetsAsync(List<Guid> termSheetIds);
        Task<TermSheetRankingResult> RankTermSheetsAsync(List<Guid> termSheetIds);
    }
}
