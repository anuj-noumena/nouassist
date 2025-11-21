using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Constants;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Entities;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Enums;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Events;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Aggregates
{
    /// <summary>
    /// Aggregate root representing a marketing channel's performance
    /// </summary>
    public class MarketingChannelPerformance : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// Tenant ID for multi-tenancy support
        /// </summary>
        public Guid? TenantId { get; protected set; }
        
        /// <summary>
        /// Channel name
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// Channel description
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// Channel type
        /// </summary>
        public MarketingChannelType Type { get; private set; }
        
        /// <summary>
        /// Channel details
        /// </summary>
        public ChannelDetails Details { get; private set; }
        
        /// <summary>
        /// Whether the channel is active
        /// </summary>
        public bool IsActive { get; private set; }
        
        /// <summary>
        /// Cost per acquisition metrics
        /// </summary>
        public CostPerAcquisition CostPerAcquisition { get; private set; }
        
        /// <summary>
        /// Return on ad spend metrics
        /// </summary>
        public ReturnOnAdSpend ReturnOnAdSpend { get; private set; }
        
        /// <summary>
        /// Metrics for this channel
        /// </summary>
        private readonly List<ChannelMetric> _metrics;
        public IReadOnlyCollection<ChannelMetric> Metrics => _metrics.AsReadOnly();
        
        protected MarketingChannelPerformance()
        {
            _metrics = new List<ChannelMetric>();
        }
        
        /// <summary>
        /// Creates a new marketing channel
        /// </summary>
        public static MarketingChannelPerformance Create(
            Guid id,
            string name, 
            string description, 
            MarketingChannelType type, 
            ChannelDetails details,
            Guid? tenantId = null)
        {
            var channel = new MarketingChannelPerformance
            {
                Id = id,
                TenantId = tenantId,
                IsActive = true
            };
            
            channel.SetName(name);
            channel.Description = description;
            channel.Type = type;
            channel.Details = details;
            
            return channel;
        }
        
        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name, 
                nameof(name), 
                maxLength: GrowthExperimentationConstants.MarketingChannels.MaxChannelNameLength
            );
        }
        
        /// <summary>
        /// Updates the channel details
        /// </summary>
        public void UpdateDetails(ChannelDetails details)
        {
            Details = details;
        }
        
        /// <summary>
        /// Sets the channel as active or inactive
        /// </summary>
        public void SetActive(bool isActive)
        {
            IsActive = isActive;
        }
        
        /// <summary>
        /// Adds a metric to the channel
        /// </summary>
        public ChannelMetric AddMetric(
            Guid metricId,
            string name, 
            MetricType type, 
            double initialValue, 
            double targetValue,
            DateTime createdAt)
        {
            // Check for duplicate metric name
            if (_metrics.Any(m => m.Name == name))
            {
                throw new BusinessException("GrowthExperimentation:ErrorMessage:DuplicateMetricName")
                    .WithData("metricName", name);
            }
            
            var metric = new ChannelMetric(metricId, Id, name, type, initialValue, targetValue, createdAt);
            _metrics.Add(metric);
            
            return metric;
        }
        
        /// <summary>
        /// Updates a metric value
        /// </summary>
        public void UpdateMetricValue(Guid metricId, double newValue, DateTime updateTime)
        {
            var metric = _metrics.FirstOrDefault(m => m.Id == metricId);
            
            if (metric == null)
            {
                throw new BusinessException("GrowthExperimentation:ErrorMessage:MetricNotFound")
                    .WithData("metricId", metricId);
            }
            
            double? previousValue = metric.Value;
            metric.UpdateValue(newValue, updateTime);
            
            // If significant change, publish event
            if (previousValue.HasValue && Math.Abs((newValue - previousValue.Value) / previousValue.Value) > 0.15)
            {
                AddDistributedEvent(new ChannelPerformanceChangedEto
                {
                    ChannelId = Id,
                    ChannelName = Name,
                    ChannelType = Type,
                    MetricName = metric.Name,
                    PreviousValue = previousValue.Value,
                    NewValue = newValue,
                    PercentageChange = (newValue - previousValue.Value) / previousValue.Value,
                    DetectedAt = updateTime,
                    TenantId = TenantId
                });
            }
        }
        
        /// <summary>
        /// Updates cost per acquisition metrics
        /// </summary>
        public void UpdateCostPerAcquisition(CostPerAcquisition cpa)
        {
            CostPerAcquisition = cpa;
        }
        
        /// <summary>
        /// Updates return on ad spend metrics
        /// </summary>
        public void UpdateReturnOnAdSpend(ReturnOnAdSpend roas)
        {
            ReturnOnAdSpend = roas;
        }
    }
}
