using NouAssist.RequirementsManagement.QualityAssurance.Enums;
using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.QualityAssurance.QualityMetrics.Dtos
{
    /// <summary>
    /// Data transfer object for a quality metric snapshot
    /// </summary>
    public class QualityMetricDto : EntityDto<Guid>
    {
        /// <summary>
        /// The type of metric being measured
        /// </summary>
        public MetricType Type { get; set; }
        
        /// <summary>
        /// The numeric value of the metric
        /// </summary>
        public decimal Value { get; set; }
        
        /// <summary>
        /// Unit of measurement (e.g., percentage, count, hours)
        /// </summary>
        public string Unit { get; set; }
        
        /// <summary>
        /// When the metric was captured
        /// </summary>
        public DateTime Timestamp { get; set; }
        
        /// <summary>
        /// Optional ID of the test plan this metric is associated with
        /// </summary>
        public Guid? TestPlanId { get; set; }
        
        /// <summary>
        /// Optional release version this metric is associated with
        /// </summary>
        public string ReleaseVersion { get; set; }
        
        /// <summary>
        /// Additional contextual information about the metric
        /// </summary>
        public string Context { get; set; }
    }
}
