using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
