using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.FinancialManagement.Shared.ValueObjects
{
    public class MonetaryValue : ValueObject
    {
        public decimal Amount { get; }
        public string Currency { get; }
        public int DecimalPlaces { get; }

        protected MonetaryValue() { } // For ORM

        public MonetaryValue(decimal amount, string currency)
        {
            if (currency == null) 
                throw new ArgumentNullException(nameof(currency));
            if (string.IsNullOrWhiteSpace(currency)) 
                throw new ArgumentException("Currency cannot be empty", nameof(currency));

            Currency = currency.ToUpperInvariant();
            DecimalPlaces = GetDecimalPlaces(Currency);
            Amount = Math.Round(amount, DecimalPlaces);
        }

        public MonetaryValue Add(MonetaryValue other)
        {
            ValidateSameCurrency(other);
            return new MonetaryValue(Amount + other.Amount, Currency);
        }

        public MonetaryValue Subtract(MonetaryValue other)
        {
            ValidateSameCurrency(other);
            return new MonetaryValue(Amount - other.Amount, Currency);
        }

        public MonetaryValue Multiply(decimal multiplier)
        {
            return new MonetaryValue(Amount * multiplier, Currency);
        }

        public bool IsZero => Amount == 0;
        public bool IsPositive => Amount > 0;
        public bool IsNegative => Amount < 0;

        private void ValidateSameCurrency(MonetaryValue other)
        {
            if (Currency != other.Currency)
                throw new InvalidOperationException(
                    $"Cannot perform operation on different currencies: {Currency} and {other.Currency}");
        }

        private static int GetDecimalPlaces(string currency) => currency switch
        {
            "USD" => 2,
            "EUR" => 2,
            "GBP" => 2,
            "JPY" => 0,
            "BTC" => 8,
            _ => 2
        };

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Amount;
            yield return Currency;
        }

        public override string ToString() => $"{Amount:F{DecimalPlaces}} {Currency}";
    }
}
