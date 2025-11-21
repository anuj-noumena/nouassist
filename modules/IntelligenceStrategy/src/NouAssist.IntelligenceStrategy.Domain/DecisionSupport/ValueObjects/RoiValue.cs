using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.ValueObjects
{
    /// <summary>
    /// Represents a Return on Investment value
    /// </summary>
    public class RoiValue : ValueObject
    {
        /// <summary>
        /// The ROI percentage value
        /// </summary>
        public decimal Percentage { get; private set; }
        
        /// <summary>
        /// The time period in months for the ROI calculation
        /// </summary>
        public int TimeframeMonths { get; private set; }
        
        /// <summary>
        /// The net benefit value
        /// </summary>
        public decimal NetBenefit { get; private set; }
        
        /// <summary>
        /// The total cost value
        /// </summary>
        public decimal TotalCost { get; private set; }

        private RoiValue()
        {
        }

        /// <summary>
        /// Creates a new instance of RoiValue
        /// </summary>
        /// <param name="netBenefit">The net benefit value</param>
        /// <param name="totalCost">The total cost value</param>
        /// <param name="timeframeMonths">The time period in months</param>
        public RoiValue(decimal netBenefit, decimal totalCost, int timeframeMonths)
        {
            NetBenefit = netBenefit;
            TotalCost = totalCost;
            TimeframeMonths = timeframeMonths;
            
            // Calculate ROI as a percentage: (Net Benefit / Total Cost) * 100
            Percentage = totalCost > 0 ? (netBenefit / totalCost) * 100 : 0;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Percentage;
            yield return TimeframeMonths;
            yield return NetBenefit;
            yield return TotalCost;
        }
    }
}
