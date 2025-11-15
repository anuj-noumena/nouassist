using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.FinancialManagement.Shared.ValueObjects
{
    public class MonetaryValue : ValueObject
    {
        public decimal Amount { get; private set; }
        public string Currency { get; private set; }
        public int DecimalPlaces { get; private set; }

        protected MonetaryValue() { } // EF Core

        public MonetaryValue(decimal amount, string currency)
        {
            Currency = Check.NotNullOrWhiteSpace(currency, nameof(currency)).ToUpperInvariant();
            DecimalPlaces = GetDecimalPlaces(currency);
            Amount = Math.Round(amount, DecimalPlaces);
            
            ValidateCurrency(currency);
        }

        public MonetaryValue Add(MonetaryValue other)
        {
            if (Currency != other.Currency)
                throw new BusinessException("Financial:CurrencyMismatch", 
                    $"Cannot add different currencies: {Currency} and {other.Currency}");
            
            return new MonetaryValue(Amount + other.Amount, Currency);
        }

        public MonetaryValue Subtract(MonetaryValue other)
        {
            if (Currency != other.Currency)
                throw new BusinessException("Financial:CurrencyMismatch", 
                    $"Cannot subtract different currencies: {Currency} and {other.Currency}");
            
            return new MonetaryValue(Amount - other.Amount, Currency);
        }

        public MonetaryValue Multiply(decimal multiplier)
        {
            return new MonetaryValue(Amount * multiplier, Currency);
        }

        public MonetaryValue Divide(decimal divisor)
        {
            if (divisor == 0)
                throw new BusinessException("Financial:DivisionByZero", 
                    "Cannot divide monetary value by zero");
            
            return new MonetaryValue(Amount / divisor, Currency);
        }

        public bool IsZero => Amount == 0;
        public bool IsPositive => Amount > 0;
        public bool IsNegative => Amount < 0;
        public MonetaryValue Abs() => new MonetaryValue(Math.Abs(Amount), Currency);

        private static int GetDecimalPlaces(string currency)
        {
            return currency switch
            {
                "USD" => 2,
                "EUR" => 2,
                "GBP" => 2,
                "JPY" => 0,
                "BTC" => 8,
                "ETH" => 8,
                _ => 2
            };
        }

        private static void ValidateCurrency(string currency)
        {
            var validCurrencies = new[] { "USD", "EUR", "GBP", "JPY", "CAD", "AUD", "CHF", "CNY", "BTC", "ETH" };
            if (Array.IndexOf(validCurrencies, currency) == -1)
                throw new BusinessException("Financial:InvalidCurrency", 
                    $"Currency '{currency}' is not supported");
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Amount;
            yield return Currency;
        }

        public override string ToString() => $"{Amount:F{DecimalPlaces}} {Currency}";

        public static MonetaryValue Zero(string currency) => new MonetaryValue(0, currency);
    }
}
