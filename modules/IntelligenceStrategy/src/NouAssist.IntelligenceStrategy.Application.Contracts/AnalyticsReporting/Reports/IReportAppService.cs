using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Reports.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Reports
{
    /// <summary>
    /// Application service for report management
    /// </summary>
    public interface IReportAppService : IApplicationService
    {
        /// <summary>
        /// Gets a report by ID
        /// </summary>
        Task<ReportConfigurationDto> GetAsync(Guid id);
        
        /// <summary>
        /// Gets a paginated list of reports
        /// </summary>
        Task<PagedResultDto<ReportConfigurationDto>> GetListAsync(GetReportListDto input);
        
        /// <summary>
        /// Creates a new report
        /// </summary>
        Task<ReportConfigurationDto> CreateAsync(CreateReportConfigurationDto input);
        
        /// <summary>
        /// Updates an existing report
        /// </summary>
        Task<ReportConfigurationDto> UpdateAsync(Guid id, UpdateReportConfigurationDto input);
        
        /// <summary>
        /// Deletes a report
        /// </summary>
        Task DeleteAsync(Guid id);
        
        /// <summary>
        /// Adds a parameter to a report
        /// </summary>
        Task<ReportParameterDto> AddParameterAsync(Guid reportId, AddReportParameterDto input);
        
        /// <summary>
        /// Updates a parameter on a report
        /// </summary>
        Task<ReportParameterDto> UpdateParameterAsync(Guid reportId, string parameterName, UpdateReportParameterDto input);
        
        /// <summary>
        /// Removes a parameter from a report
        /// </summary>
        Task DeleteParameterAsync(Guid reportId, string parameterName);
        
        /// <summary>
        /// Adds a schedule to a report
        /// </summary>
        Task<ReportScheduleDto> AddScheduleAsync(Guid reportId, AddReportScheduleDto input);
        
        /// <summary>
        /// Updates a report schedule
        /// </summary>
        Task<ReportScheduleDto> UpdateScheduleAsync(Guid reportId, Guid scheduleId, UpdateReportScheduleDto input);
        
        /// <summary>
        /// Removes a schedule from a report
        /// </summary>
        Task DeleteScheduleAsync(Guid reportId, Guid scheduleId);
        
        /// <summary>
        /// Generates a report based on configuration
        /// </summary>
        Task<ReportGenerationLogDto> GenerateReportAsync(Guid reportId, GenerateReportDto input);
        
        /// <summary>
        /// Gets report generation logs for a report
        /// </summary>
        Task<PagedResultDto<ReportGenerationLogDto>> GetGenerationLogsAsync(Guid reportId, GetReportGenerationLogListDto input);
    }
}
