using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.TaxManagement.ValueObjects
{
    public class TaxBracket : ValueObject
    {
        public decimal MinIncome { get; init; }
        public decimal MaxIncome { get; init; }
        public decimal TaxRate { get; init; }
        public decimal BaseTax { get; init; }

        public TaxBracket(
            decimal minIncome,
            decimal maxIncome,
            decimal taxRate,
            decimal baseTax)
        {
            MinIncome = minIncome;
            MaxIncome = maxIncome;
            TaxRate = taxRate;
            BaseTax = baseTax;
        }

        public bool IsInBracket(decimal income)
        {
            return income >= MinIncome && income < MaxIncome;
        }

        public decimal CalculateTax(decimal income)
        {
            if (!IsInBracket(income)) return 0;
            return BaseTax + ((income - MinIncome) * TaxRate);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return MinIncome;
            yield return MaxIncome;
            yield return TaxRate;
            yield return BaseTax;
        }
    }
}
