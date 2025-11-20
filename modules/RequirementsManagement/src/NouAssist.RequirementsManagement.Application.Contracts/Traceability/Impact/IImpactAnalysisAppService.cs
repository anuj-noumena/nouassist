using System.Threading.Tasks;
using NouAssist.RequirementsManagement.Traceability.Impact.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.RequirementsManagement.Traceability.Impact
{
    /// <summary>
    /// Application service interface for impact analysis
    /// </summary>
    public interface IImpactAnalysisAppService : IApplicationService
    {
        /// <summary>
        /// Performs impact analysis for a specific artifact
        /// </summary>
        Task<ImpactAnalysisResultDto> AnalyzeImpactAsync(GetImpactAnalysisDto input);
    }
}
