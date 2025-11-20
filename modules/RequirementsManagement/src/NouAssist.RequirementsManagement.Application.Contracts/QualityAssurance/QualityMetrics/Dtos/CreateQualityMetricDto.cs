using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.QualityAssurance.Enums;

namespace NouAssist.RequirementsManagement.QualityAssurance.QualityMetrics.Dtos
{
    /// <summary>
    /// Input for creating a new quality metric snapshot
    /// </summary>
    public class CreateQualityMetricDto
    {
        /// <summary>
        /// The type of metric being measured
        /// </summary>
        [Required]
        public MetricType Type { get; set; }
        
        /// <summary>
        /// The numeric value of the metric
        /// </summary>
        [Required]
        [Range(0, 100000)]
        public decimal Value { get; set; }
        
        /// <summary>
        /// Unit of measurement (e.g., percentage, count, hours)
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Unit { get; set; }
        
        /// <summary>
        /// Optional ID of the test plan this metric is associated with
        /// </summary>
        public Guid? TestPlanId { get; set; }
        
        /// <summary>
        /// Optional release version this metric is associated with
        /// </summary>
        [StringLength(100)]
        public string ReleaseVersion { get; set; }
        
        /// <summary>
        /// Additional contextual information about the metric
        /// </summary>
        [StringLength(500)]
        public string Context { get; set; }
    }
}
