using System;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.MarketingChannels.Dtos
{
    public class ReturnOnAdSpendDto
    {
        public decimal AdSpend { get; set; }
        public decimal Revenue { get; set; }
        public decimal Value { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
    }
}
