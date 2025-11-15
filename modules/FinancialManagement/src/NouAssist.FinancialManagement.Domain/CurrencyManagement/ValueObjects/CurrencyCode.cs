using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.CurrencyManagement.ValueObjects
{
    public class CurrencyCode : ValueObject
    {
        public string Code { get; init; }

        private static readonly HashSet<string> ValidCurrencyCodes = new()
        {
            "USD", "EUR", "GBP", "JPY", "CHF", "CAD", "AUD", "NZD",
            "CNY", "INR", "BRL", "ZAR", "MXN", "SGD", "HKD", "SEK",
            "NOK", "DKK", "PLN", "THB", "IDR", "MYR", "PHP", "CZK"
            // Add more as needed
        };

        protected CurrencyCode() { }

        public CurrencyCode(string code)
        {
            code = Check.NotNullOrWhiteSpace(code, nameof(code)).ToUpperInvariant();
            
            if (!Regex.IsMatch(code, "^[A-Z]{3}$"))
            {
                throw new BusinessException("CurrencyManagement:InvalidCurrencyCode");
            }

            // Optional: Validate against ISO 4217
            if (!ValidCurrencyCodes.Contains(code))
            {
                // Log warning but allow for extensibility
            }

            Code = code;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Code;
        }

        public override string ToString() => Code;

        public static implicit operator string(CurrencyCode currencyCode) => currencyCode.Code;
        public static implicit operator CurrencyCode(string code) => new(code);
    }
}
