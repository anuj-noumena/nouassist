using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.FinancialManagement.TaxManagement.ValueObjects
{
    public class DeductionLimit : ValueObject
    {
        public decimal MaxAmount { get; init; }
        public decimal PercentageOfIncome { get; init; }
        public bool HasPhaseOut { get; init; }
        public decimal PhaseOutStartIncome { get; init; }
        public decimal PhaseOutRate { get; init; }

        public DeductionLimit(
            decimal maxAmount,
            decimal percentageOfIncome,
            bool hasPhaseOut = false,
            decimal phaseOutStartIncome = 0,
            decimal phaseOutRate = 0)
        {
            MaxAmount = maxAmount;
            PercentageOfIncome = percentageOfIncome;
            HasPhaseOut = hasPhaseOut;
            PhaseOutStartIncome = phaseOutStartIncome;
            PhaseOutRate = phaseOutRate;
        }

        public decimal CalculateAllowedDeduction(decimal claimedAmount, decimal income)
        {
            var incomeBasedLimit = income * PercentageOfIncome;
            var allowedAmount = System.Math.Min(claimedAmount, System.Math.Min(MaxAmount, incomeBasedLimit));

            if (HasPhaseOut && income > PhaseOutStartIncome)
            {
                var phaseOutAmount = (income - PhaseOutStartIncome) * PhaseOutRate;
                allowedAmount = System.Math.Max(0, allowedAmount - phaseOutAmount);
            }

            return allowedAmount;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return MaxAmount;
            yield return PercentageOfIncome;
            yield return HasPhaseOut;
            yield return PhaseOutStartIncome;
            yield return PhaseOutRate;
        }
    }
}
