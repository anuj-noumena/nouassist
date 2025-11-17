using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Domain.Values;

namespace NouAssist.FinancialManagement.InvestmentManagement.ValueObjects
{
    public class PortfolioAllocation : ValueObject
    {
        public Dictionary<string, decimal> AssetClassPercentages { get; init; }

        public PortfolioAllocation(Dictionary<string, decimal> assetClassPercentages)
        {
            AssetClassPercentages = assetClassPercentages;
        }

        public bool IsBalanced(decimal tolerancePercentage = 5m)
        {
            var total = AssetClassPercentages.Values.Sum();
            return System.Math.Abs(total - 100m) <= tolerancePercentage;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            foreach (var kvp in AssetClassPercentages.OrderBy(x => x.Key))
            {
                yield return kvp.Key;
                yield return kvp.Value;
            }
        }
    }
}
