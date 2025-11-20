using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Events;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Exceptions;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Aggregates
{
    /// <summary>
    /// Definition of a Key Performance Indicator, its calculation, thresholds, and alerts
    /// </summary>
    public class KpiDefinition : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// Optional tenant ID for multi-tenancy
        /// </summary>
        public Guid? TenantId { get; protected set; }
        
        /// <summary>
        /// Name of the KPI
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// Description of what the KPI measures
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// Category of the KPI
        /// </summary>
        public string Category { get; private set; }
        
        /// <summary>
        /// Units of measurement for the KPI
        /// </summary>
        public string Unit { get; private set; }
        
        /// <summary>
        /// Data sources used to calculate the KPI
        /// </summary>
        public Guid[] DataSourceIds { get; private set; }
        
        /// <summary>
        /// SQL or query definition to calculate the KPI
        /// </summary>
        public string CalculationDefinition { get; private set; }
        
        /// <summary>
        /// Format string for displaying the KPI value
        /// </summary>
        public string DisplayFormat { get; private set; }
        
        /// <summary>
        /// How often the KPI should be recalculated
        /// </summary>
        public RefreshInterval RefreshInterval { get; private set; }
        
        /// <summary>
        /// The current value of the KPI
        /// </summary>
        public KpiValue CurrentValue { get; private set; }
        
        /// <summary>
        /// Whether this KPI is active
        /// </summary>
        public bool IsActive { get; private set; }
        
        /// <summary>
        /// Tags for organizing KPIs
        /// </summary>
        public string[] Tags { get; private set; }
        
        private readonly List<KpiThreshold> _thresholds;
        
        /// <summary>
        /// Thresholds for the KPI
        /// </summary>
        public IReadOnlyCollection<KpiThreshold> Thresholds => _thresholds.AsReadOnly();
        
        protected KpiDefinition()
        {
            _thresholds = new List<KpiThreshold>();
            DataSourceIds = Array.Empty<Guid>();
            Tags = Array.Empty<string>();
        }
        
        /// <summary>
        /// Creates a new KPI definition
        /// </summary>
        public static KpiDefinition Create(
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
            string[] tags = null,
            Guid? tenantId = null)
        {
            var kpi = new KpiDefinition(
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
                tags,
                tenantId
            );
            
            kpi.AddDistributedEvent(
                new KpiDefinedEto
                {
                    Id = kpi.Id,
                    Name = kpi.Name,
                    Description = kpi.Description,
                    CreatorId = kpi.CreatorId.GetValueOrDefault(),
                    HasAlerts = false,
                    TenantId = kpi.TenantId
                }
            );
            
            return kpi;
        }
        
        private KpiDefinition(
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
            string[] tags = null,
            Guid? tenantId = null)
            : base(id)
        {
            SetName(name);
            SetDescription(description);
            Category = category;
            Unit = unit;
            DataSourceIds = dataSourceIds ?? Array.Empty<Guid>();
            CalculationDefinition = calculationDefinition;
            RefreshInterval = refreshInterval;
            DisplayFormat = displayFormat;
            IsActive = isActive;
            Tags = tags ?? Array.Empty<string>();
            TenantId = tenantId;
            _thresholds = new List<KpiThreshold>();
        }
        
        /// <summary>
        /// Sets the KPI name
        /// </summary>
        public void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), AnalyticsReportingConsts.MaxNameLength);
        }
        
        /// <summary>
        /// Sets the KPI description
        /// </summary>
        public void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                AnalyticsReportingConsts.MaxDescriptionLength
            );
        }
        
        /// <summary>
        /// Sets the KPI category
        /// </summary>
        public void SetCategory(string category)
        {
            Category = category;
        }
        
        /// <summary>
        /// Sets the KPI unit of measurement
        /// </summary>
        public void SetUnit(string unit)
        {
            Unit = unit;
        }
        
        /// <summary>
        /// Updates the data sources for the KPI
        /// </summary>
        public void UpdateDataSources(Guid[] dataSourceIds)
        {
            DataSourceIds = dataSourceIds ?? Array.Empty<Guid>();
        }
        
        /// <summary>
        /// Updates the calculation definition for the KPI
        /// </summary>
        public void UpdateCalculationDefinition(string calculationDefinition)
        {
            CalculationDefinition = Check.NotNullOrWhiteSpace(calculationDefinition, nameof(calculationDefinition));
        }
        
        /// <summary>
        /// Updates the refresh interval for the KPI
        /// </summary>
        public void UpdateRefreshInterval(RefreshInterval refreshInterval)
        {
            RefreshInterval = refreshInterval;
        }
        
        /// <summary>
        /// Sets the display format for the KPI
        /// </summary>
        public void SetDisplayFormat(string displayFormat)
        {
            DisplayFormat = displayFormat;
        }
        
        /// <summary>
        /// Activates or deactivates the KPI
        /// </summary>
        public void SetActive(bool isActive)
        {
            IsActive = isActive;
        }
        
        /// <summary>
        /// Sets tags for the KPI
        /// </summary>
        public void SetTags(string[] tags)
        {
            Tags = tags ?? Array.Empty<string>();
        }
        
        /// <summary>
        /// Updates the current value of the KPI
        /// </summary>
        public void UpdateValue(decimal value, DateTime timestamp, KpiStatus? status = null)
        {
            KpiStatus newStatus = status ?? DetermineStatus(value);
            
            if (CurrentValue != null && newStatus != CurrentValue.Status)
            {
                // Status changed, check for threshold breaches
                CheckForThresholdBreaches(value, newStatus, timestamp);
            }
            
            string formattedValue = null;
            if (!string.IsNullOrEmpty(DisplayFormat))
            {
                formattedValue = string.Format(DisplayFormat, value);
            }
            
            CurrentValue = new KpiValue(value, timestamp, newStatus, formattedValue);
            
            AddDistributedEvent(
                new KpiValueUpdatedEto
                {
                    Id = Id,
                    KpiName = Name,
                    Value = value,
                    Status = (int)newStatus,
                    Timestamp = timestamp,
                    TenantId = TenantId
                }
            );
        }
        
        /// <summary>
        /// Determine the status based on thresholds
        /// </summary>
        private KpiStatus DetermineStatus(decimal value)
        {
            if (_thresholds.Count == 0)
            {
                return KpiStatus.Ok;
            }
            
            // Check critical thresholds first
            var criticalThresholds = _thresholds
                .Where(t => t.Status == KpiStatus.Critical)
                .OrderByDescending(t => t.Value);
                
            foreach (var threshold in criticalThresholds)
            {
                if (threshold.IsThresholdMet(value))
                {
                    return KpiStatus.Critical;
                }
            }
            
            // Then check warning thresholds
            var warningThresholds = _thresholds
                .Where(t => t.Status == KpiStatus.Warning)
                .OrderByDescending(t => t.Value);
                
            foreach (var threshold in warningThresholds)
            {
                if (threshold.IsThresholdMet(value))
                {
                    return KpiStatus.Warning;
                }
            }
            
            return KpiStatus.Ok;
        }
        
        /// <summary>
        /// Check for threshold breaches and raise events if needed
        /// </summary>
        private void CheckForThresholdBreaches(decimal value, KpiStatus newStatus, DateTime timestamp)
        {
            if (newStatus == KpiStatus.Warning || newStatus == KpiStatus.Critical)
            {
                // Find the threshold that was breached
                var breachedThreshold = _thresholds
                    .Where(t => t.Status == newStatus)
                    .FirstOrDefault(t => t.IsThresholdMet(value));
                    
                if (breachedThreshold != null)
                {
                    AddDistributedEvent(
                        new KpiThresholdBreachedEto
                        {
                            Id = Guid.NewGuid(),
                            KpiDefinitionId = Id,
                            KpiName = Name,
                            CurrentValue = value,
                            ThresholdValue = breachedThreshold.Value,
                            Status = newStatus,
                            BreachTime = timestamp,
                            TenantId = TenantId
                        }
                    );
                }
            }
        }
        
        /// <summary>
        /// Adds a threshold to the KPI
        /// </summary>
        public KpiThreshold AddThreshold(
            Guid thresholdId,
            decimal value,
            string @operator,
            KpiStatus status,
            AlertConditionLogic alertCondition = null,
            bool sendNotification = false,
            string[] notificationRecipients = null)
        {
            var threshold = new KpiThreshold(
                thresholdId,
                Id,
                value,
                @operator,
                status,
                alertCondition,
                sendNotification,
                notificationRecipients
            );
            
            _thresholds.Add(threshold);
            
            AddDistributedEvent(
                new KpiThresholdCreatedEto
                {
                    Id = threshold.Id,
                    KpiDefinitionId = Id,
                    Status = (int)status,
                    TenantId = TenantId
                }
            );
            
            return threshold;
        }
        
        /// <summary>
        /// Updates a threshold for the KPI
        /// </summary>
        public void UpdateThreshold(
            Guid thresholdId,
            decimal? value = null,
            string @operator = null,
            KpiStatus? status = null,
            AlertConditionLogic alertCondition = null,
            bool? sendNotification = null,
            string[] notificationRecipients = null)
        {
            var threshold = GetThreshold(thresholdId);
            
            if (value.HasValue)
            {
                threshold.UpdateThresholdValue(value.Value);
            }
            
            if (@operator != null)
            {
                threshold.UpdateOperator(@operator);
            }
            
            if (status.HasValue)
            {
                threshold.UpdateStatus(status.Value);
            }
            
            if (alertCondition != null)
            {
                threshold.SetAlertCondition(alertCondition);
            }
            
            if (sendNotification.HasValue || notificationRecipients != null)
            {
                threshold.SetNotificationSettings(
                    sendNotification ?? threshold.SendNotification,
                    notificationRecipients
                );
            }
            
            AddDistributedEvent(
                new KpiThresholdUpdatedEto
                {
                    Id = threshold.Id,
                    KpiDefinitionId = Id,
                    TenantId = TenantId
                }
            );
        }
        
        /// <summary>
        /// Removes a threshold from the KPI
        /// </summary>
        public void RemoveThreshold(Guid thresholdId)
        {
            var threshold = GetThreshold(thresholdId);
            _thresholds.Remove(threshold);
            
            AddDistributedEvent(
                new KpiThresholdDeletedEto
                {
                    Id = thresholdId,
                    KpiDefinitionId = Id,
                    TenantId = TenantId
                }
            );
        }
        
        /// <summary>
        /// Gets a threshold by ID
        /// </summary>
        private KpiThreshold GetThreshold(Guid thresholdId)
        {
            var threshold = _thresholds.FirstOrDefault(t => t.Id == thresholdId);
            if (threshold == null)
            {
                throw new EntityNotFoundException("Threshold", thresholdId);
            }
            return threshold;
        }
        
        /// <summary>
        /// Creates an alert log for a threshold breach
        /// </summary>
        public KpiAlertLog CreateAlertLog(
            Guid logId,
            Guid thresholdId,
            DateTime timestamp,
            decimal value,
            KpiStatus status,
            string message = null)
        {
            GetThreshold(thresholdId); // Validate threshold exists
            
            return new KpiAlertLog(
                logId,
                Id,
                thresholdId,
                timestamp,
                value,
                status,
                message
            );
        }
    }
}
