using System;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.MarketingChannels.Dtos
{
    public class CostPerAcquisitionDto
    {
        public decimal Cost { get; set; }
        public int Acquisitions { get; set; }
        public decimal Value { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
    }
}
