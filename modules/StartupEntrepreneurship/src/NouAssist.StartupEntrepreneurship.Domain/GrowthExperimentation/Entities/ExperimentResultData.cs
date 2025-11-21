using System;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Enums;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Entities
{
    /// <summary>
    /// Entity that represents result data for an experiment variant
    /// </summary>
    public class ExperimentResultData : Entity<Guid>
    {
        /// <summary>
        /// The variant ID this result belongs to
        /// </summary>
        public Guid VariantId { get; private set; }
        
        /// <summary>
        /// Name of the metric
        /// </summary>
        public string MetricName { get; private set; }
        
        /// <summary>
        /// Type of the metric
        /// </summary>
        public MetricType MetricType { get; private set; }
        
        /// <summary>
        /// Result value details
        /// </summary>
        public ResultValue Value { get; private set; }
        
        /// <summary>
        /// Whether this is a primary metric
        /// </summary>
        public bool IsPrimary { get; private set; }
        
        /// <summary>
        /// When this result was recorded
        /// </summary>
        public DateTime RecordedAt { get; private set; }
        
        protected ExperimentResultData() { }
        
        internal ExperimentResultData(
            Guid id,
            Guid variantId, 
            string metricName, 
            MetricType metricType, 
            ResultValue value, 
            bool isPrimary,
            DateTime recordedAt)
            : base(id)
        {
            VariantId = variantId;
            MetricName = Check.NotNullOrWhiteSpace(metricName, nameof(metricName));
            MetricType = metricType;
            Value = value;
            IsPrimary = isPrimary;
            RecordedAt = recordedAt;
        }
        
        internal void UpdateValue(ResultValue value, DateTime recordedAt)
        {
            Value = value;
            RecordedAt = recordedAt;
        }
    }
}
