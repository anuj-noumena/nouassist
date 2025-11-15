using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.TaxManagement.ValueObjects
{
    public class TaxCalculation : ValueObject
    {
        public decimal GrossIncome { get; init; }
        public decimal AdjustedGrossIncome { get; init; }
        public decimal TaxableIncome { get; init; }
        public decimal TotalTaxLiability { get; init; }
        public decimal EffectiveTaxRate { get; init; }
        public decimal MarginalTaxRate { get; init; }
        public DateTime CalculationDate { get; init; }

        public TaxCalculation(
            decimal grossIncome,
            decimal adjustedGrossIncome,
            decimal taxableIncome,
            decimal totalTaxLiability,
            decimal effectiveTaxRate,
            decimal marginalTaxRate,
            DateTime calculationDate)
        {
            GrossIncome = grossIncome;
            AdjustedGrossIncome = adjustedGrossIncome;
            TaxableIncome = taxableIncome;
            TotalTaxLiability = totalTaxLiability;
            EffectiveTaxRate = effectiveTaxRate;
            MarginalTaxRate = marginalTaxRate;
            CalculationDate = calculationDate;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return GrossIncome;
            yield return AdjustedGrossIncome;
            yield return TaxableIncome;
            yield return TotalTaxLiability;
            yield return EffectiveTaxRate;
            yield return MarginalTaxRate;
            yield return CalculationDate;
        }
    }
}
