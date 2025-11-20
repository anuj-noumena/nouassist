using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.InvestorUpdates.Dtos
{
    /// <summary>
    /// DTO representing a metric snapshot
    /// </summary>
    public class MetricSnapshotDto : EntityDto<Guid>
    {
        public Guid UpdateId { get; set; }
        public string MetricName { get; set; }
        public decimal MetricValue { get; set; }
        public string MetricUnit { get; set; }
        public decimal? MetricPreviousValue { get; set; }
        public decimal? MetricTargetValue { get; set; }
        public DateTime SnapshotDate { get; set; }

        // Calculated properties
        public decimal? GrowthPercentage { get; set; }
        public decimal? TargetAchievementPercentage { get; set; }
    }
}
