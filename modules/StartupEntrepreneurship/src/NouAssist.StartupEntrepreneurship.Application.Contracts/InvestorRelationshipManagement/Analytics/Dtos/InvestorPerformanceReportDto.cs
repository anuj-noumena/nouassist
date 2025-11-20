using System;
using System.Collections.Generic;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Analytics.Dtos
{
    /// <summary>
    /// DTO representing investor performance report
    /// </summary>
    public class InvestorPerformanceReportDto
    {
        public Guid InvestorId { get; set; }
        public string InvestorName { get; set; }
        public int RelationshipScore { get; set; }
        public decimal TotalInvestment { get; set; }
        public int InteractionCount { get; set; }
        public DateTime? LastInteraction { get; set; }
        public decimal EngagementRate { get; set; }
        public List<string> RecentActivities { get; set; } = new();
    }
}
