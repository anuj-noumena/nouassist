using System;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Entities
{
    /// <summary>
    /// Entity that represents a metric for a marketing channel
    /// </summary>
    public class ChannelMetric : Entity<Guid>
    {
        /// <summary>
        /// The channel ID this metric belongs to
        /// </summary>
        public Guid ChannelId { get; private set; }
        
        /// <summary>
        /// Name of the metric
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// Type of the metric
        /// </summary>
        public MetricType Type { get; private set; }
        
        /// <summary>
        /// Current value of the metric
        /// </summary>
        public double Value { get; private set; }
        
        /// <summary>
        /// Target value for this metric
        /// </summary>
        public double TargetValue { get; private set; }
        
        /// <summary>
        /// Previous value for comparison
        /// </summary>
        public double? PreviousValue { get; private set; }
        
        /// <summary>
        /// Change percentage from previous value
        /// </summary>
        public double? ChangePercentage { get; private set; }
        
        /// <summary>
        /// When this metric was last updated
        /// </summary>
        public DateTime LastUpdated { get; private set; }
        
        protected ChannelMetric() { }
        
        internal ChannelMetric(
            Guid id,
            Guid channelId, 
            string name, 
            MetricType type, 
            double value, 
            double targetValue,
            DateTime lastUpdated)
            : base(id)
        {
            ChannelId = channelId;
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            Type = type;
            Value = value;
            TargetValue = targetValue;
            LastUpdated = lastUpdated;
        }
        
        internal void UpdateValue(double newValue, DateTime updateTime)
        {
            PreviousValue = Value;
            Value = newValue;
            
            if (PreviousValue.HasValue && PreviousValue.Value != 0)
            {
                ChangePercentage = (newValue - PreviousValue.Value) / PreviousValue.Value;
            }
            
            LastUpdated = updateTime;
        }
        
        internal void UpdateTarget(double newTarget)
        {
            TargetValue = newTarget;
        }
    }
}
