using System;
using System.Threading.Tasks;
using NouAssist.BusinessOperations.ProjectExecution.Reports.Dtos;

namespace NouAssist.BusinessOperations.ProjectExecution.Reports
{
    /// <summary>
    /// Interface for report application service
    /// </summary>
    public interface IReportAppService
    {
        /// <summary>
        /// Generates a project report
        /// </summary>
        Task<ProjectReportDto> GenerateProjectReportAsync(Guid projectId);
        
        /// <summary>
        /// Generates a resource utilization report
        /// </summary>
        Task<ResourceUtilizationReportDto> GenerateResourceUtilizationReportAsync(DateTime startDate, DateTime endDate);
    }
}
