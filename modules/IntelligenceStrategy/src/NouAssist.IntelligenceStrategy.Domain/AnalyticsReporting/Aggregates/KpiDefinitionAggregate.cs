using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.Analytics.Constants;
using NouAssist.Analytics.Entities;
using NouAssist.Analytics.Enums;
using NouAssist.Analytics.Events;
using NouAssist.Analytics.Exceptions;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Entities;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Enums;
using NouAssist.IntelligenceStrategy.AnalyticsReporting.Events;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Analytics.Aggregates
{
    /// <summary>
    /// Represents a KPI definition with thresholds, data sources, and calculation configuration
    /// </summary>
    public class KpiDefinition : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        
        public string Name { get; private set; }
        
        public string Description { get; private set; }
        
        public string CalculationExpression { get; private set; }
        
        public decimal CurrentValue { get; private set; }
        
        public KpiStatus Status { get; private set; }
        
        public DateTime LastUpdated { get; private set; }
        
        private readonly List<KpiThreshold> _thresholds;
        public IReadOnlyCollection<KpiThreshold> Thresholds => _thresholds.AsReadOnly();
        
        private readonly List<Guid> _dataSources;
        public IReadOnlyCollection<Guid> DataSources => _dataSources.AsReadOnly();
        
        private readonly List<KpiAlertLog> _alertLogs;
        public IReadOnlyCollection<KpiAlertLog> AlertLogs => _alertLogs.AsReadOnly();
        
        protected KpiDefinition()
        {
            _thresholds = new List<KpiThreshold>();
            _dataSources = new List<Guid>();
            _alertLogs = new List<KpiAlertLog>();
        }
        
        public KpiDefinition(
            Guid id,
            string name,
            string description,
            string calculationExpression,
            Guid? tenantId
        ) : base(id)
        {
            TenantId = tenantId;
            SetName(name);
            SetDescription(description);
            CalculationExpression = Check.NotNullOrWhiteSpace(calculationExpression, nameof(calculationExpression));
            Status = KpiStatus.NotAvailable;
            CurrentValue = 0;
            LastUpdated = DateTime.UtcNow;
            _thresholds = new List<KpiThreshold>();
            _dataSources = new List<Guid>();
            _alertLogs = new List<KpiAlertLog>();
        }
        
        public void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: AnalyticsConsts.Kpi.MaxNameLength
            );
        }
        
        public void SetDescription(string description)
        {
            Description = Check.Length(
                description,
                nameof(description),
                maxLength: AnalyticsConsts.Kpi.MaxDescriptionLength
            );
        }
        
        public void SetCalculationExpression(string calculationExpression)
        {
            CalculationExpression = Check.NotNullOrWhiteSpace(calculationExpression, nameof(calculationExpression));
        }
        
        public void AddThreshold(KpiThreshold threshold)
        {
            Check.NotNull(threshold, nameof(threshold));
            
            if (_thresholds.Count >= AnalyticsConsts.Kpi.MaxThresholdsPerKpi)
            {
                throw new InvalidKpiDefinitionException(
                    Id, 
                    $"Maximum allowed thresholds ({AnalyticsConsts.Kpi.MaxThresholdsPerKpi}) exceeded"
                );
            }
            
            _thresholds.Add(threshold);
        }
        
        public void RemoveThreshold(Guid thresholdId)
        {
            var threshold = _thresholds.FirstOrDefault(t => t.Id == thresholdId);
            if (threshold == null)
            {
                return;
            }
            
            _thresholds.Remove(threshold);
        }
        
        public void AddDataSource(Guid dataSourceId)
        {
            if (!_dataSources.Contains(dataSourceId))
            {
                _dataSources.Add(dataSourceId);
            }
        }
        
        public void RemoveDataSource(Guid dataSourceId)
        {
            _dataSources.Remove(dataSourceId);
        }
        
        public void UpdateValue(decimal newValue, DateTime updateTime)
        {
            CurrentValue = newValue;
            LastUpdated = updateTime;
            
            // Evaluate thresholds to determine status
            UpdateStatus();
        }
        
        private void UpdateStatus()
        {
            if (!_thresholds.Any())
            {
                Status = KpiStatus.NotAvailable;
                return;
            }
            
            // Find the most severe threshold that has been breached
            var breachedThresholds = _thresholds
                .Where(t => t.IsBreached(CurrentValue))
                .OrderByDescending(t => t.Severity)
                .ToList();
            
            if (breachedThresholds.Any())
            {
                var mostSevereThreshold = breachedThresholds.First();
                Status = mostSevereThreshold.StatusWhenBreached;
                
                // Log alert and raise event
                var alertLog = new KpiAlertLog(
                    Guid.NewGuid(),
                    Id,
                    CurrentValue,
                    mostSevereThreshold.Id,
                    mostSevereThreshold.ThresholdValue,
                    Status,
                    DateTime.UtcNow
                );
                
                _alertLogs.Add(alertLog);
                
                // Only keep the latest 10 alert logs
                while (_alertLogs.Count > 10)
                {
                    _alertLogs.RemoveAt(0);
                }
                
                // Raise domain event
                AddDistributedEvent(new KpiThresholdBreachedEto
                {
                    Id = alertLog.Id,
                    KpiDefinitionId = Id,
                    KpiName = Name,
                    Status = Status,
                    CurrentValue = CurrentValue,
                    ThresholdValue = mostSevereThreshold.ThresholdValue,
                    OccurredAt = alertLog.OccurredAt,
                    Description = $"KPI '{Name}' has breached the {mostSevereThreshold.ThresholdValue} threshold"
                });
            }
            else
            {
                Status = KpiStatus.Ok;
            }
        }
    }
}
