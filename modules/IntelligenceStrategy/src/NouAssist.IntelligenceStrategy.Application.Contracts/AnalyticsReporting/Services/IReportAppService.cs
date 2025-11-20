using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.Analytics.DTOs.Report;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Analytics.Services
{
    /// <summary>
    /// Application service interface for report operations
    /// </summary>
    public interface IReportAppService : IApplicationService
    {
        Task<ReportConfigurationDto> GetAsync(Guid id);
        
        Task<PagedResultDto<ReportConfigurationDto>> GetListAsync(PagedAndSortedResultRequestDto input);
        
        Task<ReportConfigurationDto> CreateAsync(CreateReportConfigurationDto input);
        
        Task<ReportConfigurationDto> UpdateAsync(Guid id, UpdateReportConfigurationDto input);
        
        Task DeleteAsync(Guid id);
        
        Task UpdateScheduleAsync(Guid id, UpdateReportScheduleDto input);
        
        Task<ReportParameterDto> AddParameterAsync(Guid id, AddReportParameterDto input);
        
        Task RemoveParameterAsync(Guid id, string parameterName);
        
        Task AddDataSourceAsync(Guid id, Guid dataSourceId);
        
        Task RemoveDataSourceAsync(Guid id, Guid dataSourceId);
        
        Task<GeneratedReportDto> GenerateReportAsync(Guid id, GenerateReportDto input);
        
        Task<List<ReportGenerationLogDto>> GetGenerationLogsAsync(Guid id, int maxCount = 10);
    }
}
