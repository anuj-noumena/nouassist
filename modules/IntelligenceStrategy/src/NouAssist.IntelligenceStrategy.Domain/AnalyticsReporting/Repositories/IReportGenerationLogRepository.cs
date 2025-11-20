using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Repositories
{
    /// <summary>
    /// Repository for report generation logs
    /// </summary>
    public interface IReportGenerationLogRepository : IRepository<ReportGenerationLog, Guid>
    {
        /// <summary>
        /// Gets logs for a specific report
        /// </summary>
        Task<List<ReportGenerationLog>> GetLogsForReportAsync(Guid reportId, int maxResultCount = 10);
        
        /// <summary>
        /// Gets logs for a specific schedule
        /// </summary>
        Task<List<ReportGenerationLog>> GetLogsForScheduleAsync(Guid scheduleId, int maxResultCount = 10);
    }
}
