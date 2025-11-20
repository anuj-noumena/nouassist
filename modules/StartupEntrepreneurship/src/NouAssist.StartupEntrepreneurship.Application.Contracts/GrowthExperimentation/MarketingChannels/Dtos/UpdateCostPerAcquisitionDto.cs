using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.MarketingChannels.Dtos
{
    public class UpdateCostPerAcquisitionDto
    {
        [Range(0, double.MaxValue)]
        public decimal Cost { get; set; }
        
        [Range(0, int.MaxValue)]
        public int Acquisitions { get; set; }
        
        public DateTime PeriodStart { get; set; }
        
        public DateTime PeriodEnd { get; set; }
    }
}
