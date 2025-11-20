using System.Collections.Generic;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.RevenueModel.Dtos
{
    public class PricingTierDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public List<string> Features { get; set; }
        public int? MaxUsers { get; set; }
        public string BillingPeriod { get; set; }

        public PricingTierDto()
        {
            Features = new List<string>();
        }
    }
}
