using System;
using System.Threading.Tasks;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Aggregates;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Interfaces.Services
{
    /// <summary>
    /// Interface for KPI management operations
    /// </summary>
    public interface IKpiManager : IDomainService
    {
        /// <summary>
        /// Creates a new KPI definition
        /// </summary>
        Task<KpiDefinition> CreateKpiAsync(
            Guid id,
            string name,
            string description,
            string category,
            string unit,
            Guid[] dataSourceIds,
            string calculationDefinition,
            RefreshInterval refreshInterval,
            string displayFormat = null,
            bool isActive = true,
            Guid? tenantId = null);
            
        /// <summary>
        /// Adds a threshold to a KPI
        /// </summary>
        Task<KpiThreshold> AddThresholdToKpiAsync(
            Guid thresholdId,
            Guid kpiId,
            decimal value,
            string @operator,
            KpiStatus status,
            AlertConditionLogic alertCondition = null,
            bool sendNotification = false,
            string[] notificationRecipients = null);
            
        /// <summary>
        /// Updates the value of a KPI
        /// </summary>
        Task UpdateKpiValueAsync(
            Guid kpiId,
            decimal value,
            DateTime timestamp,
            KpiStatus? status = null);
    }
}
