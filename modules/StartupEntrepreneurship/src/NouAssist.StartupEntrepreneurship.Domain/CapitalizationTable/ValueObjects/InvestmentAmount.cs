using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.ValueObjects
{
    /// <summary>
    /// Represents an investment amount with currency
    /// </summary>
    public class InvestmentAmount : ValueObject
    {
        public decimal Value { get; private set; }
        public string Currency { get; private set; }

        private InvestmentAmount() { }

        public InvestmentAmount(decimal value, string currency = "USD")
        {
            SetValue(value);
            Currency = Check.NotNullOrWhiteSpace(currency, nameof(currency));
        }

        private void SetValue(decimal value)
        {
            if (value < 0)
            {
                throw new BusinessException("CapTable:Validation:GreaterThanZero")
                    .WithData("propertyName", "Investment Amount");
            }

            Value = Math.Round(value, 2);
        }

        public InvestmentAmount Add(InvestmentAmount other)
        {
            if (Currency != other.Currency)
            {
                throw new BusinessException("CapTable:ErrorMessage:InvalidValue")
                    .WithData("reason", "Cannot add amounts with different currencies");
            }

            return new InvestmentAmount(Value + other.Value, Currency);
        }

        public InvestmentAmount Subtract(InvestmentAmount other)
        {
            if (Currency != other.Currency)
            {
                throw new BusinessException("CapTable:ErrorMessage:InvalidValue")
                    .WithData("reason", "Cannot subtract amounts with different currencies");
            }

            return new InvestmentAmount(Value - other.Value, Currency);
        }

        public static implicit operator decimal(InvestmentAmount amount)
        {
            return amount.Value;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
            yield return Currency;
        }

        public override string ToString()
        {
            return $"{Currency} {Value:N2}";
        }
    }
}
