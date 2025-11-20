using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// EF Core implementation of the IReportGenerationLogRepository
    /// </summary>
    public class EfCoreReportGenerationLogRepository : 
        EfCoreRepository<IAnalyticsReportingDbContext, ReportGenerationLog, Guid>,
        IReportGenerationLogRepository
    {
        public EfCoreReportGenerationLogRepository(IDbContextProvider<IAnalyticsReportingDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        /// <summary>
        /// Gets logs for a specific report
        /// </summary>
        public async Task<List<ReportGenerationLog>> GetLogsForReportAsync(Guid reportId, int maxResultCount = 10)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.ReportGenerationLogs
                .Where(l => l.ReportConfigurationId == reportId)
                .OrderByDescending(l => l.StartTime)
                .Take(maxResultCount)
                .ToListAsync();
        }

        /// <summary>
        /// Gets logs for a specific schedule
        /// </summary>
        public async Task<List<ReportGenerationLog>> GetLogsForScheduleAsync(Guid scheduleId, int maxResultCount = 10)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.ReportGenerationLogs
                .Where(l => l.ReportScheduleId == scheduleId)
                .OrderByDescending(l => l.StartTime)
                .Take(maxResultCount)
                .ToListAsync();
        }
    }
}
