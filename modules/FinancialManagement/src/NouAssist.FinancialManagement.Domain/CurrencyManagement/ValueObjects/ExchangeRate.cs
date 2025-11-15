using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.CurrencyManagement.ValueObjects
{
    public class ExchangeRate : ValueObject
    {
        public decimal Rate { get; init; }
        public decimal InverseRate { get; init; }
        public DateTime EffectiveDate { get; init; }
        public int DecimalPlaces { get; init; }

        protected ExchangeRate() { }

        public ExchangeRate(decimal rate, DateTime effectiveDate, int decimalPlaces = 6)
        {
            if (rate <= 0)
            {
                throw new BusinessException("CurrencyManagement:InvalidExchangeRate");
            }

            Rate = Math.Round(rate, decimalPlaces);
            InverseRate = Math.Round(1 / rate, decimalPlaces);
            EffectiveDate = effectiveDate.Date;
            DecimalPlaces = decimalPlaces;
        }

        public decimal Convert(decimal amount, bool inverse = false)
        {
            var rateToUse = inverse ? InverseRate : Rate;
            return Math.Round(amount * rateToUse, 2); // Typically round to 2 decimal places for money
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Rate;
            yield return EffectiveDate;
        }
    }
}
