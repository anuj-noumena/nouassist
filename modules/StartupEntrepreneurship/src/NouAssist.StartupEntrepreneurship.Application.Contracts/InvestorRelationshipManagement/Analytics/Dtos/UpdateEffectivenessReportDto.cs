using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Enums;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Analytics.Dtos
{
    /// <summary>
    /// DTO representing update effectiveness report
    /// </summary>
    public class UpdateEffectivenessReportDto
    {
        public int TotalUpdatesSent { get; set; }
        public decimal AverageDeliveryRate { get; set; }
        public decimal AverageOpenRate { get; set; }
        public decimal AverageEngagementRate { get; set; }
        public List<UpdatePerformanceDto> TopPerformingUpdates { get; set; } = new();
        public Dictionary<UpdateFrequency, decimal> PerformanceByFrequency { get; set; } = new();
    }

    public class UpdatePerformanceDto
    {
        public Guid UpdateId { get; set; }
        public string Title { get; set; }
        public DateTime SentDate { get; set; }
        public decimal DeliveryRate { get; set; }
        public decimal OpenRate { get; set; }
        public decimal EngagementRate { get; set; }
    }
}
