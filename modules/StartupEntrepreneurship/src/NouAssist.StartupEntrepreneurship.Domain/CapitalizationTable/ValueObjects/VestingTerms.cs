using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.ValueObjects
{
    /// <summary>
    /// Represents vesting terms for securities
    /// </summary>
    public class VestingTerms : ValueObject
    {
        public int VestingPeriodMonths { get; private set; }
        public int CliffMonths { get; private set; }
        public decimal VestingPercentage { get; private set; }

        private VestingTerms() { }

        public VestingTerms(int vestingPeriodMonths, int cliffMonths = 0, decimal vestingPercentage = 100m)
        {
            SetVestingPeriodMonths(vestingPeriodMonths);
            SetCliffMonths(cliffMonths);
            SetVestingPercentage(vestingPercentage);
        }

        private void SetVestingPeriodMonths(int value)
        {
            if (value <= 0)
            {
                throw new BusinessException("CapTable:Validation:GreaterThanZero")
                    .WithData("propertyName", "Vesting Period");
            }

            VestingPeriodMonths = value;
        }

        private void SetCliffMonths(int value)
        {
            if (value < 0 || value >= VestingPeriodMonths)
            {
                throw new BusinessException("CapTable:ErrorMessage:InvalidVestingSchedule")
                    .WithData("reason", "Cliff period must be less than vesting period");
            }

            CliffMonths = value;
        }

        private void SetVestingPercentage(decimal value)
        {
            if (value <= 0 || value > 100)
            {
                throw new BusinessException("CapTable:Validation:BetweenRange")
                    .WithData("propertyName", "Vesting Percentage")
                    .WithData("min", 0)
                    .WithData("max", 100);
            }

            VestingPercentage = value;
        }

        public decimal CalculateVestedAmount(decimal totalAmount, DateTime grantDate, DateTime currentDate)
        {
            var monthsElapsed = ((currentDate.Year - grantDate.Year) * 12) + currentDate.Month - grantDate.Month;

            if (monthsElapsed < CliffMonths)
                return 0;

            if (monthsElapsed >= VestingPeriodMonths)
                return totalAmount * (VestingPercentage / 100);

            var vestingRatio = (decimal)monthsElapsed / VestingPeriodMonths;
            return totalAmount * vestingRatio * (VestingPercentage / 100);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return VestingPeriodMonths;
            yield return CliffMonths;
            yield return VestingPercentage;
        }

        public override string ToString()
        {
            return $"{VestingPeriodMonths} months, {CliffMonths} cliff, {VestingPercentage}% vesting";
        }
    }
}
