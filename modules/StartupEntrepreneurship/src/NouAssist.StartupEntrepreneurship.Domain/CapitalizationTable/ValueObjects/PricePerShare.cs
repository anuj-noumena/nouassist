using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Constants;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.ValueObjects
{
    /// <summary>
    /// Represents the price per share
    /// </summary>
    public class PricePerShare : ValueObject
    {
        public decimal Value { get; private set; }
        public string Currency { get; private set; }

        private PricePerShare() { }

        public PricePerShare(decimal value, string currency = "USD")
        {
            SetValue(value);
            Currency = Check.NotNullOrWhiteSpace(currency, nameof(currency));
        }

        private void SetValue(decimal value)
        {
            if (value <= 0 || value < CapTableConsts.MinPricePerShare)
            {
                throw new BusinessException("CapTable:ErrorMessage:InvalidPricePerShare");
            }

            if (value > CapTableConsts.MaxPricePerShare)
            {
                throw new BusinessException("CapTable:Validation:BetweenRange")
                    .WithData("propertyName", "Price Per Share")
                    .WithData("min", CapTableConsts.MinPricePerShare)
                    .WithData("max", CapTableConsts.MaxPricePerShare);
            }

            Value = Math.Round(value, 4);
        }

        public InvestmentAmount CalculateInvestment(ShareQuantity quantity)
        {
            return new InvestmentAmount(Value * quantity.Value, Currency);
        }

        public static implicit operator decimal(PricePerShare price)
        {
            return price.Value;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
            yield return Currency;
        }

        public override string ToString()
        {
            return $"{Currency} {Value:0.####}";
        }
    }
}
