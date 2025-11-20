using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.MarketingChannels.Dtos
{
    public class UpdateReturnOnAdSpendDto
    {
        [Range(0, double.MaxValue)]
        public decimal AdSpend { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal Revenue { get; set; }
        
        public DateTime PeriodStart { get; set; }
        
        public DateTime PeriodEnd { get; set; }
    }
}
