using System.Collections.Generic;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.RevenueModel.Dtos
{
    public class ScenarioParametersDto
    {
        public int CustomerCount { get; set; }
        public decimal AverageRevenuePerUser { get; set; }
        public double ChurnRate { get; set; }
        public double GrowthRate { get; set; }
        public int TimeHorizonMonths { get; set; }
        public decimal CostPerCustomer { get; set; }
        public Dictionary<string, object> AdditionalParameters { get; set; }

        public ScenarioParametersDto()
        {
            AdditionalParameters = new Dictionary<string, object>();
        }
    }
}
