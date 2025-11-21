using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Aggregates;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Exceptions;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Interfaces.Services;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Localization;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Repositories;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.Localization;
using Microsoft.Extensions.Localization;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Services
{
    /// <summary>
    /// Manages operations on reports
    /// </summary>
    public class ReportManager : DomainService, IReportManager
    {
        private readonly IReportConfigurationRepository _reportRepository;
        private readonly IDataSourceRepository _dataSourceRepository;
        private readonly IReportGenerationLogRepository _logRepository;
        private readonly ILogger<ReportManager> _logger;
        
        public ReportManager(
            IReportConfigurationRepository reportRepository,
            IDataSourceRepository dataSourceRepository,
            IReportGenerationLogRepository logRepository,
            ILogger<ReportManager> logger)
        {
            _reportRepository = reportRepository;
            _dataSourceRepository = dataSourceRepository;
            _logRepository = logRepository;
            _logger = logger;
        }
        
        /// <summary>
        /// Creates a new report configuration
        /// </summary>
        public async Task<ReportConfiguration> CreateReportAsync(
            Guid id,
            string name,
            string description,
            string category,
            Guid[] dataSourceIds,
            string queryDefinition,
            ReportFormat defaultFormat,
            string templateDefinition = null,
            ExportSettings exportSettings = null,
            Guid? tenantId = null)
        {
            _logger.LogInformation("Creating report {Name}", name);
            
            // Check for duplicate name
            if (await _reportRepository.ExistsByNameAsync(name, tenantId))
            {
                throw new DuplicateNameException(L["AnalyticsReporting:Entity:Report"], name);
            }
            
            // Validate data sources
            await ValidateDataSourcesAsync(dataSourceIds, tenantId);
            
            var report = ReportConfiguration.Create(
                id,
                name,
                description,
                category,
                dataSourceIds,
                queryDefinition,
                defaultFormat,
                templateDefinition,
                exportSettings,
                null,
                tenantId
            );
            
            await _reportRepository.InsertAsync(report);
            
            return report;
        }
        
        /// <summary>
        /// Validates that data sources exist and are accessible
        /// </summary>
        private async Task ValidateDataSourcesAsync(Guid[] dataSourceIds, Guid? tenantId)
        {
            if (dataSourceIds == null || dataSourceIds.Length == 0)
            {
                throw new BusinessException("AnalyticsReporting:ErrorMessage:InvalidReportConfiguration")
                    .WithData("reason", "At least one data source is required");
            }
            
            foreach (var dataSourceId in dataSourceIds)
            {
                var dataSource = await _dataSourceRepository.GetAsync(dataSourceId);
                
                // Check if data source is accessible for this tenant
                if (dataSource.TenantId.HasValue && dataSource.TenantId != tenantId)
                {
                    throw new BusinessException("AnalyticsReporting:ErrorMessage:InvalidReportConfiguration")
                        .WithData("reason", $"Data source {dataSource.Name} is not accessible for this tenant");
                }
                
                // Check if data source is active
                if (!dataSource.IsActive)
                {
                    throw new BusinessException("AnalyticsReporting:ErrorMessage:InvalidReportConfiguration")
                        .WithData("reason", $"Data source {dataSource.Name} is not active");
                }
            }
        }
        
        /// <summary>
        /// Adds a schedule to a report
        /// </summary>
        public async Task<ReportSchedule> AddScheduleToReportAsync(
            Guid scheduleId,
            Guid reportId,
            ScheduleTiming timing,
            ReportFormat format,
            string[] recipients = null,
            bool attachToEmail = true,
            string emailSubject = null,
            string emailBody = null)
        {
            var report = await _reportRepository.GetAsync(reportId);
            
            // Validate email settings
            if (recipients != null && recipients.Length > 0 && !attachToEmail && string.IsNullOrEmpty(emailBody))
            {
                throw new BusinessException("AnalyticsReporting:ErrorMessage:InvalidReportConfiguration")
                    .WithData("reason", "Email body is required when not attaching report to email");
            }
            
            var schedule = report.AddSchedule(
                scheduleId,
                timing,
                format,
                recipients,
                attachToEmail,
                emailSubject,
                emailBody
            );
            
            await _reportRepository.UpdateAsync(report);
            
            return schedule;
        }
        
        /// <summary>
        /// Generates a report based on configuration
        /// </summary>
        public async Task<ReportGenerationLog> GenerateReportAsync(
            Guid reportId,
            ReportFormat format,
            Dictionary<string, string> parameters = null,
            Guid? scheduleId = null,
            Guid? userId = null)
        {
            var report = await _reportRepository.GetAsync(reportId);
            
            // Validate parameters
            ValidateReportParameters(report, parameters);
            
            // Create generation log
            var logId = GuidGenerator.Create();
            var startTime = DateTime.UtcNow;
            string parametersJson = parameters != null ? System.Text.Json.JsonSerializer.Serialize(parameters) : null;
            
            var log = report.CreateGenerationLog(
                logId,
                startTime,
                format,
                scheduleId,
                userId,
                parametersJson
            );
            
            await _logRepository.InsertAsync(log);
            
            try
            {
                // In a real implementation, this would execute the report generation
                // For this example, we'll just simulate it
                await Task.Delay(100);
                
                // Mark as completed
                log.MarkAsCompleted(DateTime.UtcNow, "reports/result_path.pdf");
                await _logRepository.UpdateAsync(log);
                
                // If this was triggered by a schedule, record the execution
                if (scheduleId.HasValue)
                {
                    report.RecordScheduleExecution(scheduleId.Value, startTime, CalculateNextScheduledTime(report, scheduleId.Value));
                    await _reportRepository.UpdateAsync(report);
                }
                
                return log;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating report {ReportId}", reportId);
                
                // Mark as failed
                log.MarkAsFailed(DateTime.UtcNow, ex.Message);
                await _logRepository.UpdateAsync(log);
                
                throw new BusinessException("AnalyticsReporting:ErrorMessage:ReportGenerationFailed")
                    .WithData("reason", ex.Message);
            }
        }
        
        /// <summary>
        /// Validates report parameters
        /// </summary>
        private void ValidateReportParameters(ReportConfiguration report, Dictionary<string, string> parameters)
        {
            // Check required parameters
            foreach (var param in report.Parameters)
            {
                if (param.IsRequired && (parameters == null || !parameters.ContainsKey(param.Name) || string.IsNullOrEmpty(parameters[param.Name])))
                {
                    throw new BusinessException("AnalyticsReporting:ErrorMessage:InvalidReportConfiguration")
                        .WithData("reason", $"Required parameter {param.Name} is missing");
                }
                
                // More validation could be added here, like type checking, etc.
            }
        }
        
        /// <summary>
        /// Calculates the next scheduled time for a report
        /// </summary>
        private DateTime? CalculateNextScheduledTime(ReportConfiguration report, Guid scheduleId)
        {
            // In a real implementation, this would calculate the next run time based on the cron expression
            // For this example, we'll just return a time 24 hours in the future
            return DateTime.UtcNow.AddHours(24);
        }
        
        // Other implementation methods...
        
        private IStringLocalizer<AnalyticsReportingResource> L => LazyServiceProvider.LazyGetRequiredService<IStringLocalizer<AnalyticsReportingResource>>();
    }
}
