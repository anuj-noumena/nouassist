namespace NouAssist.StartupEntrepreneurship.PerformanceFinancialModeling.ValueObjects
{
    /// <summary>
    /// Represents the break-even analysis result
    /// </summary>
    public class BreakEvenResult : ValueObject
    {
        public DateTime? BreakEvenDate { get; private set; }
        public int? BreakEvenMonthIndex { get; private set; }
        public decimal BreakEvenRevenue { get; private set; }
        public decimal BreakEvenUnits { get; private set; }
        public decimal FixedCosts { get; private set; }
        public decimal VariableCostPerUnit { get; private set; }
        public decimal PricePerUnit { get; private set; }

        private BreakEvenResult() { }

        public BreakEvenResult(DateTime? breakEvenDate, int? breakEvenMonthIndex, 
                             decimal breakEvenRevenue, decimal breakEvenUnits,
                             decimal fixedCosts, decimal variableCostPerUnit, decimal pricePerUnit)
        {
            BreakEvenDate = breakEvenDate;
            BreakEvenMonthIndex = breakEvenMonthIndex;
            BreakEvenRevenue = breakEvenRevenue;
            BreakEvenUnits = breakEvenUnits;
            FixedCosts = fixedCosts;
            VariableCostPerUnit = variableCostPerUnit;
            PricePerUnit = pricePerUnit;
        }

        public bool IsBreakEvenAchievable()
        {
            return BreakEvenDate.HasValue;
        }

        public decimal GetContributionMargin()
        {
            return PricePerUnit - VariableCostPerUnit;
        }

        public decimal GetContributionMarginRatio()
        {
            return PricePerUnit != 0 ? GetContributionMargin() / PricePerUnit : 0;
        }

        public decimal GetMarginOfSafety(decimal actualRevenue)
        {
            return actualRevenue - BreakEvenRevenue;
        }

        public decimal GetMarginOfSafetyPercentage(decimal actualRevenue)
        {
            return actualRevenue != 0 ? (GetMarginOfSafety(actualRevenue) / actualRevenue) * 100 : 0;
        }

        public BreakEvenResult WithNewPricing(decimal newPricePerUnit)
        {
            var newContributionMargin = newPricePerUnit - VariableCostPerUnit;
            var newBreakEvenUnits = newContributionMargin != 0 ? FixedCosts / newContributionMargin : 0;
            var newBreakEvenRevenue = newBreakEvenUnits * newPricePerUnit;
            
            return new BreakEvenResult(BreakEvenDate, BreakEvenMonthIndex, newBreakEvenRevenue, 
                                     newBreakEvenUnits, FixedCosts, VariableCostPerUnit, newPricePerUnit);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return BreakEvenDate;
            yield return BreakEvenMonthIndex;
            yield return BreakEvenRevenue;
            yield return BreakEvenUnits;
            yield return FixedCosts;
            yield return VariableCostPerUnit;
            yield return PricePerUnit;
        }
    }
}
