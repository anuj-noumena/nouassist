using System.Threading.Tasks;
using NouAssist.RequirementsManagement.Traceability.TraceMatrix.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.RequirementsManagement.Traceability.TraceMatrix
{
    /// <summary>
    /// Application service interface for traceability matrix operations
    /// </summary>
    public interface ITraceMatrixAppService : IApplicationService
    {
        /// <summary>
        /// Generates a traceability matrix between source and target artifact types
        /// </summary>
        Task<TraceabilityMatrixDto> GenerateMatrixAsync(GetTraceabilityMatrixDto input);
        
        /// <summary>
        /// Checks coverage between source and target artifact types
        /// </summary>
        Task<CoverageReportDto> CheckCoverageAsync(GetCoverageReportDto input);
    }
}
