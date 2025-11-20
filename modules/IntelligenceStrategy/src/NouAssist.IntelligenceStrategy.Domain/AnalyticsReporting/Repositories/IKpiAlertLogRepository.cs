using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Repositories
{
    /// <summary>
    /// Repository for KPI alert logs
    /// </summary>
    public interface IKpiAlertLogRepository : IRepository<KpiAlertLog, Guid>
    {
        /// <summary>
        /// Gets alerts for a specific KPI
        /// </summary>
        Task<List<KpiAlertLog>> GetAlertsForKpiAsync(Guid kpiId, int maxResultCount = 10);
        
        /// <summary>
        /// Gets alerts by status
        /// </summary>
        Task<List<KpiAlertLog>> GetAlertsByStatusAsync(KpiStatus status, int maxResultCount = 10);
        
        /// <summary>
        /// Gets alerts for a time period
        /// </summary>
        Task<List<KpiAlertLog>> GetAlertsForPeriodAsync(DateTime startTime, DateTime endTime, int maxResultCount = 10);
    }
}
