namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.ValueObjects
{
    /// <summary>
    /// Value object that represents cost per acquisition metrics
    /// </summary>
    public class CostPerAcquisition : ValueObject
    {
        /// <summary>
        /// Cost amount
        /// </summary>
        public decimal Cost { get; private set; }
        
        /// <summary>
        /// Number of acquisitions
        /// </summary>
        public int Acquisitions { get; private set; }
        
        /// <summary>
        /// Cost per acquisition value
        /// </summary>
        public decimal Value => Acquisitions > 0 ? Cost / Acquisitions : 0;
        
        /// <summary>
        /// Period start date
        /// </summary>
        public DateTime PeriodStart { get; private set; }
        
        /// <summary>
        /// Period end date
        /// </summary>
        public DateTime PeriodEnd { get; private set; }
        
        protected CostPerAcquisition() { }
        
        public CostPerAcquisition(decimal cost, int acquisitions, DateTime periodStart, DateTime periodEnd)
        {
            SetCost(cost);
            SetAcquisitions(acquisitions);
            SetPeriod(periodStart, periodEnd);
        }
        
        private void SetCost(decimal cost)
        {
            if (cost < 0)
            {
                throw new BusinessException("GrowthExperimentation:ErrorMessage:InvalidValue")
                    .WithData("value", cost)
                    .WithData("property", "Cost");
            }
            
            Cost = cost;
        }
        
        private void SetAcquisitions(int acquisitions)
        {
            if (acquisitions < 0)
            {
                throw new BusinessException("GrowthExperimentation:ErrorMessage:InvalidValue")
                    .WithData("value", acquisitions)
                    .WithData("property", "Acquisitions");
            }
            
            Acquisitions = acquisitions;
        }
        
        private void SetPeriod(DateTime periodStart, DateTime periodEnd)
        {
            if (periodEnd < periodStart)
            {
                throw new BusinessException("GrowthExperimentation:ErrorMessage:InvalidValue")
                    .WithData("periodStart", periodStart)
                    .WithData("periodEnd", periodEnd)
                    .WithData("property", "Period");
            }
            
            PeriodStart = periodStart;
            PeriodEnd = periodEnd;
        }
        
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Cost;
            yield return Acquisitions;
            yield return PeriodStart;
            yield return PeriodEnd;
        }
    }
}
