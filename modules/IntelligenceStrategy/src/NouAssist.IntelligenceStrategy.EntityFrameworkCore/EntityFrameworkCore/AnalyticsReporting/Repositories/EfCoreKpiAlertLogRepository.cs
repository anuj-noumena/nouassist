using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// EF Core implementation of the IKpiAlertLogRepository
    /// </summary>
    public class EfCoreKpiAlertLogRepository : 
        EfCoreRepository<IAnalyticsReportingDbContext, KpiAlertLog, Guid>,
        IKpiAlertLogRepository
    {
        public EfCoreKpiAlertLogRepository(IDbContextProvider<IAnalyticsReportingDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        /// <summary>
        /// Gets alerts for a specific KPI
        /// </summary>
        public async Task<List<KpiAlertLog>> GetAlertsForKpiAsync(Guid kpiId, int maxResultCount = 10)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.KpiAlertLogs
                .Where(l => l.KpiDefinitionId == kpiId)
                .OrderByDescending(l => l.Timestamp)
                .Take(maxResultCount)
                .ToListAsync();
        }

        /// <summary>
        /// Gets alerts by status
        /// </summary>
        public async Task<List<KpiAlertLog>> GetAlertsByStatusAsync(KpiStatus status, int maxResultCount = 10)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.KpiAlertLogs
                .Where(l => l.Status == status)
                .OrderByDescending(l => l.Timestamp)
                .Take(maxResultCount)
                .ToListAsync();
        }

        /// <summary>
        /// Gets alerts for a time period
        /// </summary>
        public async Task<List<KpiAlertLog>> GetAlertsForPeriodAsync(DateTime startTime, DateTime endTime, int maxResultCount = 10)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.KpiAlertLogs
                .Where(l => l.Timestamp >= startTime && l.Timestamp <= endTime)
                .OrderByDescending(l => l.Timestamp)
                .Take(maxResultCount)
                .ToListAsync();
        }
    }
}
