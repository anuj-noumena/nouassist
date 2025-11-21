using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Constants;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.ValueObjects
{
    /// <summary>
    /// Represents a quantity of shares
    /// </summary>
    public class ShareQuantity : ValueObject
    {
        public decimal Value { get; private set; }

        private ShareQuantity() { }

        public ShareQuantity(decimal value)
        {
            SetValue(value);
        }

        private void SetValue(decimal value)
        {
            if (value < CapTableConsts.MinShareQuantity)
            {
                throw new BusinessException("CapTable:ErrorMessage:NegativeShareQuantity");
            }

            if (value > CapTableConsts.MaxShareQuantity)
            {
                throw new BusinessException("CapTable:Validation:BetweenRange")
                    .WithData("propertyName", "Share Quantity")
                    .WithData("min", CapTableConsts.MinShareQuantity)
                    .WithData("max", CapTableConsts.MaxShareQuantity);
            }

            Value = value;
        }

        public ShareQuantity Add(ShareQuantity other)
        {
            return new ShareQuantity(Value + other.Value);
        }

        public ShareQuantity Subtract(ShareQuantity other)
        {
            return new ShareQuantity(Value - other.Value);
        }

        public bool IsGreaterThan(ShareQuantity other)
        {
            return Value > other.Value;
        }

        public bool IsLessThan(ShareQuantity other)
        {
            return Value < other.Value;
        }

        public static implicit operator decimal(ShareQuantity shareQuantity)
        {
            return shareQuantity.Value;
        }

        public static implicit operator ShareQuantity(decimal value)
        {
            return new ShareQuantity(value);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        public override string ToString()
        {
            return Value.ToString("N2");
        }
    }
}
