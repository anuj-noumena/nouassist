namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.ValueObjects
{
    /// <summary>
    /// Parameters for revenue scenario simulation
    /// </summary>
    public class ScenarioParameters : ValueObject
    {
        public int CustomerCount { get; private set; }
        public decimal AverageRevenuePerUser { get; private set; }
        public double ChurnRate { get; private set; }
        public double GrowthRate { get; private set; }
        public int TimeHorizonMonths { get; private set; }
        public decimal CostPerCustomer { get; private set; }
        public Dictionary<string, object> AdditionalParameters { get; private set; }

        protected ScenarioParameters()
        {
            AdditionalParameters = new Dictionary<string, object>();
        }

        public ScenarioParameters(
            int customerCount,
            decimal averageRevenuePerUser,
            double churnRate,
            double growthRate,
            int timeHorizonMonths,
            decimal costPerCustomer = 0m,
            Dictionary<string, object> additionalParameters = null)
        {
            SetCustomerCount(customerCount);
            SetAverageRevenuePerUser(averageRevenuePerUser);
            SetChurnRate(churnRate);
            SetGrowthRate(growthRate);
            SetTimeHorizonMonths(timeHorizonMonths);
            SetCostPerCustomer(costPerCustomer);
            AdditionalParameters = additionalParameters ?? new Dictionary<string, object>();
        }

        private void SetCustomerCount(int customerCount)
        {
            if (customerCount < 0)
            {
                throw new InvalidRevenueScenarioException("CustomerCount", customerCount);
            }
            CustomerCount = customerCount;
        }

        private void SetAverageRevenuePerUser(decimal averageRevenuePerUser)
        {
            if (averageRevenuePerUser < 0)
            {
                throw new InvalidRevenueScenarioException("AverageRevenuePerUser", averageRevenuePerUser);
            }
            AverageRevenuePerUser = averageRevenuePerUser;
        }

        private void SetChurnRate(double churnRate)
        {
            if (churnRate < 0 || churnRate > 1)
            {
                throw new InvalidRevenueScenarioException("ChurnRate", churnRate);
            }
            ChurnRate = churnRate;
        }

        private void SetGrowthRate(double growthRate)
        {
            if (growthRate < -1)
            {
                throw new InvalidRevenueScenarioException("GrowthRate", growthRate);
            }
            GrowthRate = growthRate;
        }

        private void SetTimeHorizonMonths(int timeHorizonMonths)
        {
            if (timeHorizonMonths <= 0)
            {
                throw new InvalidRevenueScenarioException("TimeHorizonMonths", timeHorizonMonths);
            }
            TimeHorizonMonths = timeHorizonMonths;
        }

        private void SetCostPerCustomer(decimal costPerCustomer)
        {
            if (costPerCustomer < 0)
            {
                throw new InvalidRevenueScenarioException("CostPerCustomer", costPerCustomer);
            }
            CostPerCustomer = costPerCustomer;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return CustomerCount;
            yield return AverageRevenuePerUser;
            yield return ChurnRate;
            yield return GrowthRate;
            yield return TimeHorizonMonths;
            yield return CostPerCustomer;
            foreach (var kvp in AdditionalParameters.OrderBy(kvp => kvp.Key))
            {
                yield return kvp.Key;
                yield return kvp.Value;
            }
        }
    }
}
