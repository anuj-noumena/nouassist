using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Enums;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Analytics.Dtos
{
    /// <summary>
    /// DTO representing investor engagement report
    /// </summary>
    public class InvestorEngagementReportDto
    {
        public int TotalInvestors { get; set; }
        public int ActiveInvestors { get; set; }
        public decimal AverageEngagementRate { get; set; }
        public Dictionary<EngagementType, int> EngagementsByType { get; set; } = new();
        public List<TopInvestorEngagementDto> TopEngagedInvestors { get; set; } = new();
    }

    public class TopInvestorEngagementDto
    {
        public Guid InvestorId { get; set; }
        public string InvestorName { get; set; }
        public int TotalEngagements { get; set; }
        public decimal EngagementRate { get; set; }
    }
}
