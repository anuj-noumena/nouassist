using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.InvestorUpdates.Dtos
{
    /// <summary>
    /// DTO for creating a new metric snapshot
    /// </summary>
    public class CreateMetricSnapshotDto
    {
        [Required]
        [StringLength(200)]
        public string MetricName { get; set; }

        [Required]
        public decimal MetricValue { get; set; }

        [StringLength(50)]
        public string MetricUnit { get; set; }

        public decimal? MetricPreviousValue { get; set; }

        public decimal? MetricTargetValue { get; set; }

        public DateTime? SnapshotDate { get; set; }
    }
}
