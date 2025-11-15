using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.CurrencyManagement.ValueObjects
{
    public class MonetaryAmount : ValueObject
    {
        public decimal Amount { get; init; }
        public CurrencyCode Currency { get; init; }
        public int DecimalPlaces { get; init; }

        protected MonetaryAmount() { }

        public MonetaryAmount(decimal amount, CurrencyCode currency)
        {
            Amount = amount;
            Currency = Check.NotNull(currency, nameof(currency));
            DecimalPlaces = GetDecimalPlaces(currency.Code);
        }

        public MonetaryAmount Add(MonetaryAmount other)
        {
            if (!Currency.Equals(other.Currency))
            {
                throw new InvalidOperationException(
                    $"Cannot add amounts in different currencies: {Currency} and {other.Currency}");
            }

            return new MonetaryAmount(Amount + other.Amount, Currency);
        }

        public MonetaryAmount Subtract(MonetaryAmount other)
        {
            if (!Currency.Equals(other.Currency))
            {
                throw new InvalidOperationException(
                    $"Cannot subtract amounts in different currencies: {Currency} and {other.Currency}");
            }

            return new MonetaryAmount(Amount - other.Amount, Currency);
        }

        public MonetaryAmount Multiply(decimal multiplier)
        {
            return new MonetaryAmount(Amount * multiplier, Currency);
        }

        public bool IsZero => Amount == 0;
        public bool IsPositive => Amount > 0;
        public bool IsNegative => Amount < 0;

        private static int GetDecimalPlaces(string currencyCode)
        {
            return currencyCode switch
            {
                "JPY" => 0,  // Japanese Yen has no decimals
                "BTC" => 8,  // Bitcoin has 8 decimals
                _ => 2       // Most currencies have 2 decimals
            };
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Math.Round(Amount, DecimalPlaces);
            yield return Currency;
        }

        public override string ToString()
        {
            return $"{Amount:F{DecimalPlaces}} {Currency}";
        }
    }
}
