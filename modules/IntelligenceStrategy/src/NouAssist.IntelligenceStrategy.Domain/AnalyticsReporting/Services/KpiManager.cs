using System;
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

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Services
{
    /// <summary>
    /// Manages operations on KPIs
    /// </summary>
    public class KpiManager : DomainService, IKpiManager
    {
        private readonly IKpiDefinitionRepository _kpiRepository;
        private readonly IDataSourceRepository _dataSourceRepository;
        private readonly IKpiAlertLogRepository _alertLogRepository;
        private readonly ILogger<KpiManager> _logger;
        
        public KpiManager(
            IKpiDefinitionRepository kpiRepository,
            IDataSourceRepository dataSourceRepository,
            IKpiAlertLogRepository alertLogRepository,
            ILogger<KpiManager> logger)
        {
            _kpiRepository = kpiRepository;
            _dataSourceRepository = dataSourceRepository;
            _alertLogRepository = alertLogRepository;
            _logger = logger;
        }
        
        /// <summary>
        /// Creates a new KPI definition
        /// </summary>
        public async Task<KpiDefinition> CreateKpiAsync(
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
            Guid? tenantId = null)
        {
            _logger.LogInformation("Creating KPI {Name}", name);
            
            // Check for duplicate name
            if (await _kpiRepository.ExistsByNameAsync(name, tenantId))
            {
                throw new DuplicateNameException(L["AnalyticsReporting:Entity:KPI"], name);
            }
            
            // Validate data sources
            await ValidateDataSourcesAsync(dataSourceIds, tenantId);
            
            var kpi = KpiDefinition.Create(
                id,
                name,
                description,
                category,
                unit,
                dataSourceIds,
                calculationDefinition,
                refreshInterval,
                displayFormat,
                isActive,
                null,
                tenantId
            );
            
            await _kpiRepository.InsertAsync(kpi);
            
            return kpi;
        }
        
        /// <summary>
        /// Validates that data sources exist and are accessible
        /// </summary>
        private async Task ValidateDataSourcesAsync(Guid[] dataSourceIds, Guid? tenantId)
        {
            if (dataSourceIds == null || dataSourceIds.Length == 0)
            {
                throw new BusinessException("AnalyticsReporting:ErrorMessage:InvalidKpiConfiguration")
                    .WithData("reason", "At least one data source is required");
            }
            
            foreach (var dataSourceId in dataSourceIds)
            {
                var dataSource = await _dataSourceRepository.GetAsync(dataSourceId);
                
                // Check if data source is accessible for this tenant
                if (dataSource.TenantId.HasValue && dataSource.TenantId != tenantId)
                {
                    throw new BusinessException("AnalyticsReporting:ErrorMessage:InvalidKpiConfiguration")
                        .WithData("reason", $"Data source {dataSource.Name} is not accessible for this tenant");
                }
                
                // Check if data source is active
                if (!dataSource.IsActive)
                {
                    throw new BusinessException("AnalyticsReporting:ErrorMessage:InvalidKpiConfiguration")
                        .WithData("reason", $"Data source {dataSource.Name} is not active");
                }
            }
        }
        
        /// <summary>
        /// Adds a threshold to a KPI
        /// </summary>
        public async Task<KpiThreshold> AddThresholdToKpiAsync(
            Guid thresholdId,
            Guid kpiId,
            decimal value,
            string @operator,
            KpiStatus status,
            AlertConditionLogic alertCondition = null,
            bool sendNotification = false,
            string[] notificationRecipients = null)
        {
            var kpi = await _kpiRepository.GetAsync(kpiId);
            
            // Validate operator
            ValidateOperator(@operator);
            
            // For notification thresholds, validate recipients
            if (sendNotification && (notificationRecipients == null || notificationRecipients.Length == 0))
            {
                throw new BusinessException("AnalyticsReporting:ErrorMessage:InvalidKpiConfiguration")
                    .WithData("reason", "Notification recipients are required when notifications are enabled");
            }
            
            var threshold = kpi.AddThreshold(
                thresholdId,
                value,
                @operator,
                status,
                alertCondition,
                sendNotification,
                notificationRecipients
            );
            
            await _kpiRepository.UpdateAsync(kpi);
            
            return threshold;
        }
        
        /// <summary>
        /// Validates that an operator is valid
        /// </summary>
        private void ValidateOperator(string @operator)
        {
            string[] validOperators = { ">", ">=", "<", "<=", "==", "!=" };
            
            if (!Array.Exists(validOperators, op => op == @operator))
            {
                throw new BusinessException("AnalyticsReporting:ErrorMessage:InvalidKpiConfiguration")
                    .WithData("reason", $"Invalid operator: {@operator}");
            }
        }
        
        /// <summary>
        /// Updates the value of a KPI
        /// </summary>
        public async Task UpdateKpiValueAsync(
            Guid kpiId,
            decimal value,
            DateTime timestamp,
            KpiStatus? status = null)
        {
            var kpi = await _kpiRepository.GetAsync(kpiId);
            
            // Skip update if KPI is not active
            if (!kpi.IsActive)
            {
                _logger.LogWarning("Attempted to update inactive KPI {KpiId}", kpiId);
                return;
            }
            
            // Update the value
            kpi.UpdateValue(value, timestamp, status);
            
            // Check if we need to create alert logs
            if ((status == KpiStatus.Warning || status == KpiStatus.Critical) ||
                (kpi.CurrentValue != null && (kpi.CurrentValue.Status == KpiStatus.Warning || kpi.CurrentValue.Status == KpiStatus.Critical)))
            {
                await CreateAlertLogsIfNeededAsync(kpi, value, timestamp);
            }
            
            await _kpiRepository.UpdateAsync(kpi);
        }
        
        /// <summary>
        /// Creates alert logs for breached thresholds
        /// </summary>
        private async Task CreateAlertLogsIfNeededAsync(KpiDefinition kpi, decimal value, DateTime timestamp)
        {
            foreach (var threshold in kpi.Thresholds)
            {
                if (threshold.IsThresholdMet(value) && 
                    (threshold.Status == KpiStatus.Warning || threshold.Status == KpiStatus.Critical))
                {
                    // Create alert log
                    var logId = GuidGenerator.Create();
                    string message = $"KPI '{kpi.Name}' value {value} {threshold.Operator} threshold {threshold.Value}";
                    
                    var alertLog = kpi.CreateAlertLog(
                        logId,
                        threshold.Id,
                        timestamp,
                        value,
                        threshold.Status,
                        message
                    );
                    
                    await _alertLogRepository.InsertAsync(alertLog);
                    
                    // Notification logic would go here if needed
                    if (threshold.SendNotification && threshold.NotificationRecipients.Length > 0)
                    {
                        _logger.LogInformation(
                            "KPI alert notification would be sent to {RecipientCount} recipients for KPI {KpiName}",
                            threshold.NotificationRecipients.Length,
                            kpi.Name
                        );
                    }
                }
            }
        }
        
        // Other implementation methods...
        
        private static string L(string name)
        {
            return LocalizationResource.GetString(AnalyticsReportingResource.ResourceName, name);
        }
    }
}
