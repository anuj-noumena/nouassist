using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.ValueObjects
{
    /// <summary>
    /// Value object that represents return on ad spend metrics
    /// </summary>
    public class ReturnOnAdSpend : ValueObject
    {
        /// <summary>
        /// Ad spend amount
        /// </summary>
        public decimal AdSpend { get; private set; }
        
        /// <summary>
        /// Revenue generated
        /// </summary>
        public decimal Revenue { get; private set; }
        
        /// <summary>
        /// ROAS value (Revenue / AdSpend)
        /// </summary>
        public decimal Value => AdSpend > 0 ? Revenue / AdSpend : 0;
        
        /// <summary>
        /// Period start date
        /// </summary>
        public DateTime PeriodStart { get; private set; }
        
        /// <summary>
        /// Period end date
        /// </summary>
        public DateTime PeriodEnd { get; private set; }
        
        protected ReturnOnAdSpend() { }
        
        public ReturnOnAdSpend(decimal adSpend, decimal revenue, DateTime periodStart, DateTime periodEnd)
        {
            SetAdSpend(adSpend);
            SetRevenue(revenue);
            SetPeriod(periodStart, periodEnd);
        }
        
        private void SetAdSpend(decimal adSpend)
        {
            if (adSpend < 0)
            {
                throw new BusinessException("GrowthExperimentation:ErrorMessage:InvalidValue")
                    .WithData("value", adSpend)
                    .WithData("property", "AdSpend");
            }
            
            AdSpend = adSpend;
        }
        
        private void SetRevenue(decimal revenue)
        {
            if (revenue < 0)
            {
                throw new BusinessException("GrowthExperimentation:ErrorMessage:InvalidValue")
                    .WithData("value", revenue)
                    .WithData("property", "Revenue");
            }
            
            Revenue = revenue;
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
            yield return AdSpend;
            yield return Revenue;
            yield return PeriodStart;
            yield return PeriodEnd;
        }
    }
}
