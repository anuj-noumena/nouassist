using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Aggregates;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Exceptions;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Interfaces.Services;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Localization;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Permissions;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Reports.Dtos;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Repositories;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Services;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Reports
{
    /// <summary>
    /// Application service implementation for report management
    /// </summary>
    [Authorize(AnalyticsReportingPermissions.Report.Default)]
    public class ReportAppService : ApplicationService, IReportAppService
    {
        private readonly IReportManager _reportManager;
        private readonly IReportConfigurationRepository _reportRepository;
        private readonly IReportGenerationLogRepository _reportLogRepository;
        private readonly IDataSourceRepository _dataSourceRepository;
        private readonly ICurrentUser _currentUser;
        
        public ReportAppService(
            IReportManager reportManager,
            IReportConfigurationRepository reportRepository,
            IReportGenerationLogRepository reportLogRepository,
            IDataSourceRepository dataSourceRepository,
            ICurrentUser currentUser)
        {
            _reportManager = reportManager;
            _reportRepository = reportRepository;
            _reportLogRepository = reportLogRepository;
            _dataSourceRepository = dataSourceRepository;
            _currentUser = currentUser;
            
            // Set resource for localization
            LocalizationResource = typeof(AnalyticsReportingResource);
        }
        
        /// <summary>
        /// Gets a report by ID
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.Report.View)]
        public virtual async Task<ReportConfigurationDto> GetAsync(Guid id)
        {
            try
            {
                var report = await _reportRepository.GetAsync(id);
                
                // Check tenant
                if (report.TenantId != CurrentTenant.Id)
                {
                    throw new BusinessException(L["AnalyticsReporting:ErrorMessage:UnauthorizedAccess"]);
                }
                
                // Enrich with data source information
                var reportDto = ObjectMapper.Map<ReportConfiguration, ReportConfigurationDto>(report);
                reportDto.DataSources = await GetDataSourcesInfoAsync(report.DataSourceIds);
                
                return reportDto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Gets a paginated list of reports
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.Report.View)]
        public virtual async Task<PagedResultDto<ReportConfigurationDto>> GetListAsync(GetReportListDto input)
        {
            try
            {
                // Get queryable repository to apply filters
                var queryable = await _reportRepository.GetQueryableAsync();
                
                // Apply filters
                queryable = await ApplyListFiltersAsync(queryable, input);
                
                // Get total count
                var totalCount = await AsyncExecuter.CountAsync(queryable);
                
                // Apply sorting and paging
                queryable = queryable
                    .OrderBy(input.Sorting ?? nameof(ReportConfiguration.Name))
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount);
                
                // Get reports
                var reports = await AsyncExecuter.ToListAsync(queryable);
                
                // Map to DTOs
                var reportDtos = ObjectMapper.Map<List<ReportConfiguration>, List<ReportConfigurationDto>>(reports);
                
                // Enrich with data source information if details are requested
                if (input.IncludeDetails && reports.Any())
                {
                    foreach (var report in reports)
                    {
                        var index = reports.IndexOf(report);
                        reportDtos[index].DataSources = await GetDataSourcesInfoAsync(report.DataSourceIds);
                    }
                }
                
                return new PagedResultDto<ReportConfigurationDto>(totalCount, reportDtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Creates a new report
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.Report.Create)]
        public virtual async Task<ReportConfigurationDto> CreateAsync(CreateReportConfigurationDto input)
        {
            try
            {
                // Map export settings if provided
                ExportSettings exportSettings = null;
                if (input.ExportSettings != null)
                {
                    exportSettings = ObjectMapper.Map<CreateExportSettingsDto, ExportSettings>(input.ExportSettings);
                }
                
                // Create report using domain service
                var report = await _reportManager.CreateReportAsync(
                    GuidGenerator.Create(),
                    input.Name,
                    input.Description,
                    input.Category,
                    input.DataSourceIds,
                    input.QueryDefinition,
                    input.DefaultFormat,
                    input.TemplateDefinition,
                    exportSettings,
                    CurrentTenant.Id
                );
                
                // Map to DTO
                var reportDto = ObjectMapper.Map<ReportConfiguration, ReportConfigurationDto>(report);
                
                // Enrich with data source information
                reportDto.DataSources = await GetDataSourcesInfoAsync(report.DataSourceIds);
                
                return reportDto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Updates an existing report
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.Report.Update)]
        public virtual async Task<ReportConfigurationDto> UpdateAsync(Guid id, UpdateReportConfigurationDto input)
        {
            try
            {
                // Get the report
                var report = await _reportRepository.GetAsync(id);
                
                // Check tenant
                if (report.TenantId != CurrentTenant.Id)
                {
                    throw new BusinessException(L["AnalyticsReporting:ErrorMessage:UnauthorizedAccess"]);
                }
                
                // Update basic properties
                report.SetName(input.Name);
                report.SetDescription(input.Description);
                report.SetCategory(input.Category);
                report.UpdateDataSources(input.DataSourceIds);
                report.UpdateQueryDefinition(input.QueryDefinition);
                report.SetDefaultFormat(input.DefaultFormat);
                report.UpdateTemplateDefinition(input.TemplateDefinition);
                report.SetTags(input.Tags);
                
                // Update export settings if provided
                if (input.ExportSettings != null)
                {
                    var exportSettings = ObjectMapper.Map<UpdateExportSettingsDto, ExportSettings>(input.ExportSettings);
                    report.UpdateExportSettings(exportSettings);
                }
                
                // Save changes
                await _reportRepository.UpdateAsync(report);
                
                // Map to DTO
                var reportDto = ObjectMapper.Map<ReportConfiguration, ReportConfigurationDto>(report);
                
                // Enrich with data source information
                reportDto.DataSources = await GetDataSourcesInfoAsync(report.DataSourceIds);
                
                return reportDto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Deletes a report
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.Report.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                // Get the report
                var report = await _reportRepository.GetAsync(id);
                
                // Check tenant
                if (report.TenantId != CurrentTenant.Id)
                {
                    throw new BusinessException(L["AnalyticsReporting:ErrorMessage:UnauthorizedAccess"]);
                }
                
                // Delete the report
                await _reportRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Adds a parameter to a report
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.Report.Update)]
        public virtual async Task<ReportParameterDto> AddParameterAsync(Guid reportId, AddReportParameterDto input)
        {
            try
            {
                // Get the report
                var report = await _reportRepository.GetAsync(reportId);
                
                // Check tenant
                if (report.TenantId != CurrentTenant.Id)
                {
                    throw new BusinessException(L["AnalyticsReporting:ErrorMessage:UnauthorizedAccess"]);
                }
                
                // Add parameter
                var parameter = report.AddParameter(
                    input.Name,
                    input.DataType,
                    input.DefaultValue,
                    input.IsRequired,
                    input.DisplayName,
                    input.Description,
                    input.Order,
                    input.ValidationRules
                );
                
                // Save changes
                await _reportRepository.UpdateAsync(report);
                
                // Map to DTO
                return ObjectMapper.Map<ReportParameter, ReportParameterDto>(parameter);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Updates a parameter on a report
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.Report.Update)]
        public virtual async Task<ReportParameterDto> UpdateParameterAsync(Guid reportId, string parameterName, UpdateReportParameterDto input)
        {
            try
            {
                // Get the report
                var report = await _reportRepository.GetAsync(reportId);
                
                // Check tenant
                if (report.TenantId != CurrentTenant.Id)
                {
                    throw new BusinessException(L["AnalyticsReporting:ErrorMessage:UnauthorizedAccess"]);
                }
                
                // Update parameter
                report.UpdateParameter(
                    parameterName,
                    input.DefaultValue,
                    input.IsRequired,
                    input.DisplayName,
                    input.Description,
                    input.Order,
                    input.ValidationRules
                );
                
                // Save changes
                await _reportRepository.UpdateAsync(report);
                
                // Get updated parameter
                var parameter = report.Parameters.First(p => p.Name == parameterName);
                
                // Map to DTO
                return ObjectMapper.Map<ReportParameter, ReportParameterDto>(parameter);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Removes a parameter from a report
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.Report.Update)]
        public virtual async Task DeleteParameterAsync(Guid reportId, string parameterName)
        {
            try
            {
                // Get the report
                var report = await _reportRepository.GetAsync(reportId);
                
                // Check tenant
                if (report.TenantId != CurrentTenant.Id)
                {
                    throw new BusinessException(L["AnalyticsReporting:ErrorMessage:UnauthorizedAccess"]);
                }
                
                // Remove parameter
                report.RemoveParameter(parameterName);
                
                // Save changes
                await _reportRepository.UpdateAsync(report);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Adds a schedule to a report
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.Report.Schedule)]
        public virtual async Task<ReportScheduleDto> AddScheduleAsync(Guid reportId, AddReportScheduleDto input)
        {
            try
            {
                // Map to schedule timing value object
                var scheduleTiming = ObjectMapper.Map<AddReportScheduleDto, ScheduleTiming>(input);
                
                // Add schedule using domain service
                var schedule = await _reportManager.AddScheduleToReportAsync(
                    GuidGenerator.Create(),
                    reportId,
                    scheduleTiming,
                    input.Format,
                    input.Recipients,
                    input.AttachToEmail,
                    input.EmailSubject,
                    input.EmailBody
                );
                
                // Map to DTO
                return ObjectMapper.Map<ReportSchedule, ReportScheduleDto>(schedule);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Updates a report schedule
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.Report.Schedule)]
        public virtual async Task<ReportScheduleDto> UpdateScheduleAsync(Guid reportId, Guid scheduleId, UpdateReportScheduleDto input)
        {
            try
            {
                // Get the report
                var report = await _reportRepository.GetAsync(reportId);
                
                // Check tenant
                if (report.TenantId != CurrentTenant.Id)
                {
                    throw new BusinessException(L["AnalyticsReporting:ErrorMessage:UnauthorizedAccess"]);
                }
                
                // Get the schedule
                var schedule = report.Schedules.FirstOrDefault(s => s.Id == scheduleId);
                if (schedule == null)
                {
                    throw new EntityNotFoundException("Schedule", scheduleId);
                }
                
                // Map to schedule timing value object if cron or timezone specified
                ScheduleTiming scheduleTiming = null;
                if (!string.IsNullOrEmpty(input.CronExpression) || !string.IsNullOrEmpty(input.TimeZone) || 
                    input.StartDate.HasValue || input.EndDate.HasValue || input.IsActive.HasValue)
                {
                    // Store existing timing for mapping context
                    var existingTiming = schedule.Timing;
                    var context = new Dictionary<string, object>
                    {
                        ["ExistingTiming"] = existingTiming
                    };
                    
                    // Map input to new schedule timing
                    scheduleTiming = ObjectMapper.Map<UpdateReportScheduleDto, ScheduleTiming>(input, context);
                }
                
                // Update schedule in report aggregate
                if (scheduleTiming != null)
                {
                    report.UpdateSchedule(scheduleId, scheduleTiming);
                }
                
                // Update format if specified
                if (input.Format.HasValue)
                {
                    report.UpdateScheduleFormat(scheduleId, input.Format.Value);
                }
                
                // Update email settings if specified
                if (input.Recipients != null || input.AttachToEmail.HasValue || 
                    input.EmailSubject != null || input.EmailBody != null)
                {
                    report.UpdateScheduleEmailSettings(
                        scheduleId,
                        input.Recipients,
                        input.AttachToEmail,
                        input.EmailSubject,
                        input.EmailBody
                    );
                }
                
                // Save changes
                await _reportRepository.UpdateAsync(report);
                
                // Get updated schedule
                var updatedSchedule = report.Schedules.First(s => s.Id == scheduleId);
                
                // Map to DTO
                return ObjectMapper.Map<ReportSchedule, ReportScheduleDto>(updatedSchedule);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Removes a schedule from a report
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.Report.Schedule)]
        public virtual async Task DeleteScheduleAsync(Guid reportId, Guid scheduleId)
        {
            try
            {
                // Get the report
                var report = await _reportRepository.GetAsync(reportId);
                
                // Check tenant
                if (report.TenantId != CurrentTenant.Id)
                {
                    throw new BusinessException(L["AnalyticsReporting:ErrorMessage:UnauthorizedAccess"]);
                }
                
                // Remove schedule
                report.RemoveSchedule(scheduleId);
                
                // Save changes
                await _reportRepository.UpdateAsync(report);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Generates a report based on configuration
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.Report.Generate)]
        public virtual async Task<ReportGenerationLogDto> GenerateReportAsync(Guid reportId, GenerateReportDto input)
        {
            try
            {
                // Generate report using domain service
                var log = await _reportManager.GenerateReportAsync(
                    reportId,
                    input.Format,
                    input.Parameters,
                    null, // No schedule ID since this is an ad-hoc generation
                    _currentUser.Id
                );
                
                // Map to DTO
                return ObjectMapper.Map<ReportGenerationLog, ReportGenerationLogDto>(log);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Gets report generation logs for a report
        /// </summary>
        [Authorize(AnalyticsReportingPermissions.Report.View)]
        public virtual async Task<PagedResultDto<ReportGenerationLogDto>> GetGenerationLogsAsync(Guid reportId, GetReportGenerationLogListDto input)
        {
            try
            {
                // Get the report
                var report = await _reportRepository.GetAsync(reportId);
                
                // Check tenant
                if (report.TenantId != CurrentTenant.Id)
                {
                    throw new BusinessException(L["AnalyticsReporting:ErrorMessage:UnauthorizedAccess"]);
                }
                
                // Get queryable repository to apply filters
                var queryable = await _reportLogRepository.GetQueryableAsync();
                
                // Apply filters
                queryable = queryable.Where(l => l.ReportConfigurationId == reportId);
                
                // Apply additional filters
                if (!string.IsNullOrWhiteSpace(input.ScheduleId) && Guid.TryParse(input.ScheduleId, out var scheduleId))
                {
                    queryable = queryable.Where(l => l.ReportScheduleId == scheduleId);
                }
                
                if (!string.IsNullOrWhiteSpace(input.Status))
                {
                    queryable = queryable.Where(l => l.Status == input.Status);
                }
                
                if (!string.IsNullOrWhiteSpace(input.StartTimeMin) && DateTime.TryParse(input.StartTimeMin, out var startTimeMin))
                {
                    queryable = queryable.Where(l => l.StartTime >= startTimeMin);
                }
                
                if (!string.IsNullOrWhiteSpace(input.StartTimeMax) && DateTime.TryParse(input.StartTimeMax, out var startTimeMax))
                {
                    queryable = queryable.Where(l => l.StartTime <= startTimeMax);
                }
                
                // Get total count
                var totalCount = await AsyncExecuter.CountAsync(queryable);
                
                // Apply sorting and paging
                queryable = queryable
                    .OrderByDescending(l => l.StartTime)
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount);
                
                // Get logs
                var logs = await AsyncExecuter.ToListAsync(queryable);
                
                // Map to DTOs
                var logDtos = ObjectMapper.Map<List<ReportGenerationLog>, List<ReportGenerationLogDto>>(logs);
                
                return new PagedResultDto<ReportGenerationLogDto>(totalCount, logDtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }
        
        /// <summary>
        /// Applies filters to report list query
        /// </summary>
        private async Task<IQueryable<ReportConfiguration>> ApplyListFiltersAsync(IQueryable<ReportConfiguration> queryable, GetReportListDto input)
        {
            // Filter by current user if not admin and onlyMine is true
            if (input.OnlyMine == true)
            {
                var currentUserId = _currentUser.Id;
                if (currentUserId.HasValue)
                {
                    queryable = queryable.Where(r => r.CreatorId == currentUserId);
                }
            }
            
            // Filter by text
            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                queryable = queryable.Where(r => 
                    r.Name.Contains(input.Filter) || 
                    (r.Description != null && r.Description.Contains(input.Filter)));
            }
            
            // Filter by category
            if (!string.IsNullOrWhiteSpace(input.Category))
            {
                queryable = queryable.Where(r => r.Category == input.Category);
            }
            
            // Filter by tag
            if (!string.IsNullOrWhiteSpace(input.Tag))
            {
                // This is a simplification - in actual implementation
                // you might need a more complex query to search within the tags array
                queryable = queryable.Where(r => EF.Functions.JsonContains(r.Tags, input.Tag));
            }
            
            // Apply tenant filter
            queryable = queryable.Where(r => r.TenantId == CurrentTenant.Id);
            
            return queryable;
        }
        
        /// <summary>
        /// Gets data source information for display
        /// </summary>
        private async Task<ReportDataSourceDto[]> GetDataSourcesInfoAsync(Guid[] dataSourceIds)
        {
            if (dataSourceIds == null || dataSourceIds.Length == 0)
            {
                return Array.Empty<ReportDataSourceDto>();
            }
            
            var result = new List<ReportDataSourceDto>();
            
            foreach (var dataSourceId in dataSourceIds)
            {
                try
                {
                    var dataSource = await _dataSourceRepository.GetAsync(dataSourceId);
                    
                    result.Add(new ReportDataSourceDto
                    {
                        Id = dataSource.Id,
                        Name = dataSource.Name,
                        Type = dataSource.Type.ToString()
                    });
                }
                catch
                {
                    // If data source not found, add with unknown name
                    result.Add(new ReportDataSourceDto
                    {
                        Id = dataSourceId,
                        Name = "Unknown",
                        Type = "Unknown"
                    });
                }
            }
            
            return result.ToArray();
        }
        
        /// <summary>
        /// Error handling helper method
        /// </summary>
        private async Task HandleErrorAsync(Exception ex, LogLevel logLevel = LogLevel.Error)
        {
            // Log the error
            Logger.Log(logLevel, ex, "Error in ReportAppService: {Message}", ex.Message);
            
            // Handle specific exceptions
            if (ex is EntityNotFoundException)
            {
                throw new UserFriendlyException(
                    L["AnalyticsReporting:ErrorMessage:EntityNotFound", L["AnalyticsReporting:Entity:Report"]],
                    null,
                    ex);
            }
            
            if (ex is AnalyticsReportingDomainException domainException)
            {
                throw new UserFriendlyException(
                    L[domainException.Code, domainException.Message],
                    domainException.Code,
                    ex);
            }
            
            // For other exceptions, throw a generic error
            throw new UserFriendlyException(
                L["AnalyticsReporting:ErrorMessage:GenericError"],
                null,
                ex);
        }
    }
}
