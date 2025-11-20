using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.Analytics.Aggregates;
using NouAssist.Analytics.DTOs.Report;
using NouAssist.Analytics.Entities;
using NouAssist.Analytics.Interfaces.Services;
using NouAssist.Analytics.Permissions;
using NouAssist.Analytics.Repositories;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.Analytics.Services
{
    [Authorize(AnalyticsPermissions.Reports.Default)]
    public class ReportAppService : ApplicationService, IReportAppService
    {
        private readonly IReportManager _reportManager;
        private readonly IReportConfigurationRepository _reportRepository;
        private readonly IDataSourceRepository _dataSourceRepository;
        
        public ReportAppService(
            IReportManager reportManager,
            IReportConfigurationRepository reportRepository,
            IDataSourceRepository dataSourceRepository)
        {
            _reportManager = reportManager;
            _reportRepository = reportRepository;
            _dataSourceRepository = dataSourceRepository;
        }
        
        public async Task<ReportConfigurationDto> GetAsync(Guid id)
        {
            var report = await _reportRepository.GetAsync(id);
            return await MapReportToDtoAsync(report);
        }
        
        public async Task<PagedResultDto<ReportConfigurationDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            var totalCount = await _reportRepository.CountAsync();
            
            var reports = await _reportRepository.GetPagedListAsync(
                input.SkipCount,
                input.MaxResultCount,
                input.Sorting ?? "Name"
            );
            
            var reportDtos = new List<ReportConfigurationDto>();
            foreach (var report in reports)
            {
                reportDtos.Add(await MapReportToDtoAsync(report));
            }
            
            return new PagedResultDto<ReportConfigurationDto>(totalCount, reportDtos);
        }
        
        [Authorize(AnalyticsPermissions.Reports.Create)]
        public async Task<ReportConfigurationDto> CreateAsync(CreateReportConfigurationDto input)
        {
            var schedule = new ReportSchedule(
                input.Schedule?.IsEnabled ?? false,
                input.Schedule?.CronExpression ?? "0 0 * * *", // Default to daily at midnight
                input.Schedule?.NextRunTime ?? DateTime.UtcNow.AddDays(1).Date,
                input.Schedule?.EmailRecipients ?? "",
                input.Schedule?.StoreInRepository ?? true
            );
            
            var report = await _reportManager.CreateReportConfigurationAsync(
                input.Name,
                input.Description,
                input.DefaultFormat,
                schedule,
                CurrentTenant.Id
            );
            
            return await MapReportToDtoAsync(report);
        }
        
        [Authorize(AnalyticsPermissions.Reports.Edit)]
        public async Task<ReportConfigurationDto> UpdateAsync(Guid id, UpdateReportConfigurationDto input)
        {
            var report = await _reportManager.UpdateReportConfigurationAsync(
                id,
                input.Name,
                input.Description,
                input.DefaultFormat
            );
            
            return await MapReportToDtoAsync(report);
        }
        
        [Authorize(AnalyticsPermissions.Reports.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _reportRepository.DeleteAsync(id);
        }
        
        [Authorize(AnalyticsPermissions.Reports.Edit)]
        public async Task UpdateScheduleAsync(Guid id, UpdateReportScheduleDto input)
        {
            var report = await _reportRepository.GetAsync(id);
            
            var schedule = new ReportSchedule(
                input.IsEnabled,
                input.CronExpression,
                input.NextRunTime,
                input.EmailRecipients,
                input.StoreInRepository
            );
            
            await _reportManager.UpdateReportScheduleAsync(id, schedule);
        }
        
        [Authorize(AnalyticsPermissions.Reports.Edit)]
        public async Task<ReportParameterDto> AddParameterAsync(Guid id, AddReportParameterDto input)
        {
            var parameter = await _reportManager.AddReportParameterAsync(
                id,
                input.Name,
                input.DisplayName,
                input.DataType,
                input.IsRequired,
                input.DefaultValue,
                input.Order,
                input.ValidationRegex
            );
            
            return new ReportParameterDto
            {
                Name = parameter.Name,
                DisplayName = parameter.DisplayName,
                DataType = parameter.DataType,
                IsRequired = parameter.IsRequired,
                DefaultValue = parameter.DefaultValue,
                Order = parameter.Order,
                ValidationRegex = parameter.ValidationRegex
            };
        }
        
        [Authorize(AnalyticsPermissions.Reports.Edit)]
        public async Task RemoveParameterAsync(Guid id, string parameterName)
        {
            await _reportManager.RemoveReportParameterAsync(id, parameterName);
        }
        
        [Authorize(AnalyticsPermissions.Reports.Edit)]
        public async Task AddDataSourceAsync(Guid id, Guid dataSourceId)
        {
            // Verify data source exists
            await _dataSourceRepository.GetAsync(dataSourceId);
            
            await _reportManager.AddDataSourceToReportAsync(id, dataSourceId);
        }
        
        [Authorize(AnalyticsPermissions.Reports.Edit)]
        public async Task RemoveDataSourceAsync(Guid id, Guid dataSourceId)
        {
            await _reportManager.RemoveDataSourceFromReportAsync(id, dataSourceId);
        }
        
        [Authorize(AnalyticsPermissions.Reports.Generate)]
        public async Task<GeneratedReportDto> GenerateReportAsync(Guid id, GenerateReportDto input)
        {
            var report = await _reportRepository.GetAsync(id);
            
            var storagePath = await _reportManager.GenerateReportAsync(
                id,
                input.ParameterValues,
                input.Format,
                CurrentUser.Id
            );
            
            return new GeneratedReportDto
            {
                ReportConfigurationId = id,
                ReportName = report.Name,
                StoragePath = storagePath,
                Format = input.Format ?? report.DefaultFormat,
                GeneratedAt = DateTime.UtcNow,
                IsSuccessful = true
            };
        }
        
        public async Task<List<ReportGenerationLogDto>> GetGenerationLogsAsync(Guid id, int maxCount = 10)
        {
            var report = await _reportRepository.GetAsync(id);
            
            var logDtos = new List<ReportGenerationLogDto>();
            foreach (var log in report.GenerationLogs.OrderByDescending(l => l.GeneratedAt).Take(maxCount))
            {
                logDtos.Add(new ReportGenerationLogDto
                {
                    Id = log.Id,
                    GeneratedAt = log.GeneratedAt,
                    GeneratedBy = log.GeneratedBy,
                    GeneratedByName = log.GeneratedBy.HasValue ? await GetUserNameAsync(log.GeneratedBy.Value) : "System",
                    IsSuccessful = log.IsSuccessful,
                    Format = log.Format,
                    ErrorMessage = log.ErrorMessage,
                    StoragePath = log.StoragePath,
                    IsScheduled = log.IsScheduled,
                    Duration = log.Duration
                });
            }
            
            return logDtos;
        }
        
        private async Task<ReportConfigurationDto> MapReportToDtoAsync(ReportConfiguration report)
        {
            var dataSourceIds = report.DataSources.ToList();
            var dataSources = await _dataSourceRepository.GetListAsync(ds => dataSourceIds.Contains(ds.Id));
            
            return new ReportConfigurationDto
            {
                Id = report.Id,
                Name = report.Name,
                Description = report.Description,
                DefaultFormat = report.DefaultFormat,
                Schedule = new ReportScheduleDto
                {
                    IsEnabled = report.Schedule.IsEnabled,
                    CronExpression = report.Schedule.CronExpression,
                    NextRunTime = report.Schedule.NextRunTime,
                    LastRunTime = report.Schedule.LastRunTime,
                    EmailRecipients = report.Schedule.EmailRecipients,
                    StoreInRepository = report.Schedule.StoreInRepository
                },
                Parameters = report.Parameters.Select(p => new ReportParameterDto
                {
                    Name = p.Name,
                    DisplayName = p.DisplayName,
                    DataType = p.DataType,
                    IsRequired = p.IsRequired,
                    DefaultValue = p.DefaultValue,
                    Order = p.Order,
                    ValidationRegex = p.ValidationRegex
                }).ToList(),
                DataSources = dataSourceIds,
                DataSourceNames = dataSources.Select(ds => ds.Name).ToList(),
                RecentGenerationLogs = report.GenerationLogs.OrderByDescending(l => l.GeneratedAt).Take(5).Select(l => new ReportGenerationLogDto
                {
                    Id = l.Id,
                    GeneratedAt = l.GeneratedAt,
                    GeneratedBy = l.GeneratedBy,
                    GeneratedByName = "", // Will be populated when necessary
                    IsSuccessful = l.IsSuccessful,
                    Format = l.Format,
                    ErrorMessage = l.ErrorMessage,
                    StoragePath = l.StoragePath,
                    IsScheduled = l.IsScheduled,
                    Duration = l.Duration
                }).ToList(),
                CreationTime = report.CreationTime,
                CreatorId = report.CreatorId,
                LastModificationTime = report.LastModificationTime,
                LastModifierId = report.LastModifierId
            };
        }
        
        private async Task<string> GetUserNameAsync(Guid userId)
        {
            // In a real implementation, this would look up the user's name
            // For now, we'll just return the user ID as a string
            return userId.ToString();
        }
    }
}
