using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.RevenueModel.Dtos
{
    public class GtmPlanElementDto
    {
        public GtmChannelType ChannelType { get; set; }
        public string ChannelName { get; set; }
        public string Description { get; set; }
        public string TargetAudience { get; set; }
        public decimal EstimatedCost { get; set; }
        public string Currency { get; set; }
        public int Priority { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<string> KeyMetrics { get; set; }

        public GtmPlanElementDto()
        {
            KeyMetrics = new List<string>();
        }
    }
}
