using Volo.Abp;
using Volo.Abp.Domain.Values;
using NouAssist.StartupEntrepreneurship.CapitalizationTable.Constants;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.ValueObjects
{
    /// <summary>
    /// Represents an ownership percentage (0-100)
    /// </summary>
    public class OwnershipPercentage : ValueObject
    {
        public decimal Value { get; private set; }

        private OwnershipPercentage() { }

        public OwnershipPercentage(decimal value)
        {
            SetValue(value);
        }

        private void SetValue(decimal value)
        {
            if (value < CapTableConsts.MinOwnershipPercentage || value > CapTableConsts.MaxOwnershipPercentage)
            {
                throw new BusinessException("CapTable:ErrorMessage:InvalidOwnershipPercentage");
            }

            Value = Math.Round(value, 4);
        }

        public static OwnershipPercentage Calculate(ShareQuantity shares, ShareQuantity totalShares)
        {
            if (totalShares.Value == 0)
                return new OwnershipPercentage(0);

            var percentage = (shares.Value / totalShares.Value) * 100;
            return new OwnershipPercentage(percentage);
        }

        public static implicit operator decimal(OwnershipPercentage percentage)
        {
            return percentage.Value;
        }

        public static implicit operator OwnershipPercentage(decimal value)
        {
            return new OwnershipPercentage(value);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        public override string ToString()
        {
            return $"{Value:0.####}%";
        }
    }
}
