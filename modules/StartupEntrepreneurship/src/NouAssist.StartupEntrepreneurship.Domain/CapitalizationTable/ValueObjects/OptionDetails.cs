using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.ValueObjects
{
    /// <summary>
    /// Represents details of a stock option
    /// </summary>
    public class OptionDetails : ValueObject
    {
        public decimal StrikePrice { get; private set; }
        public DateTime? ExpiryDate { get; private set; }
        public string Currency { get; private set; }

        private OptionDetails() { }

        public OptionDetails(decimal strikePrice, DateTime? expiryDate = null, string currency = "USD")
        {
            SetStrikePrice(strikePrice);
            ExpiryDate = expiryDate;
            Currency = Check.NotNullOrWhiteSpace(currency, nameof(currency));
        }

        private void SetStrikePrice(decimal value)
        {
            if (value < 0)
            {
                throw new BusinessException("CapTable:Validation:GreaterThanZero")
                    .WithData("propertyName", "Strike Price");
            }

            StrikePrice = Math.Round(value, 4);
        }

        public bool IsExpired(DateTime currentDate)
        {
            return ExpiryDate.HasValue && currentDate > ExpiryDate.Value;
        }

        public decimal CalculateIntrinsicValue(decimal currentSharePrice)
        {
            return Math.Max(currentSharePrice - StrikePrice, 0);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return StrikePrice;
            yield return ExpiryDate;
            yield return Currency;
        }

        public override string ToString()
        {
            var expiry = ExpiryDate?.ToString("yyyy-MM-dd") ?? "No expiry";
            return $"Strike: {Currency} {StrikePrice:0.####}, Expiry: {expiry}";
        }
    }
}
