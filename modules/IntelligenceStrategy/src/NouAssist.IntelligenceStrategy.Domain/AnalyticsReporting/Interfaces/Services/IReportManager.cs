using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Aggregates;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Interfaces.Services
{
    /// <summary>
    /// Interface for report management operations
    /// </summary>
    public interface IReportManager : IDomainService
    {
        /// <summary>
        /// Creates a new report configuration
        /// </summary>
        Task<ReportConfiguration> CreateReportAsync(
            Guid id,
            string name,
            string description,
            string category,
            Guid[] dataSourceIds,
            string queryDefinition,
            ReportFormat defaultFormat,
            string templateDefinition = null,
            ExportSettings exportSettings = null,
            Guid? tenantId = null);
            
        /// <summary>
        /// Adds a schedule to a report
        /// </summary>
        Task<ReportSchedule> AddScheduleToReportAsync(
            Guid scheduleId,
            Guid reportId,
            ScheduleTiming timing,
            ReportFormat format,
            string[] recipients = null,
            bool attachToEmail = true,
            string emailSubject = null,
            string emailBody = null);
            
        /// <summary>
        /// Generates a report based on configuration
        /// </summary>
        Task<ReportGenerationLog> GenerateReportAsync(
            Guid reportId,
            ReportFormat format,
            Dictionary<string, string> parameters = null,
            Guid? scheduleId = null,
            Guid? userId = null);
    }
}
