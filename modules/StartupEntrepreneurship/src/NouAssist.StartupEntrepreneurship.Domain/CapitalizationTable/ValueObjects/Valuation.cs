using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.ValueObjects
{
    /// <summary>
    /// Represents company valuation (pre-money or post-money)
    /// </summary>
    public class Valuation : ValueObject
    {
        public decimal PreMoneyValuation { get; private set; }
        public decimal PostMoneyValuation { get; private set; }
        public string Currency { get; private set; }

        private Valuation() { }

        public Valuation(decimal preMoneyValuation, decimal investment, string currency = "USD")
        {
            Currency = Check.NotNullOrWhiteSpace(currency, nameof(currency));
            SetPreMoneyValuation(preMoneyValuation);
            PostMoneyValuation = PreMoneyValuation + investment;
        }

        public static Valuation FromPostMoney(decimal postMoneyValuation, decimal investment, string currency = "USD")
        {
            var preMoney = postMoneyValuation - investment;
            return new Valuation(preMoney, investment, currency);
        }

        private void SetPreMoneyValuation(decimal value)
        {
            if (value < 0)
            {
                throw new BusinessException("CapTable:Validation:GreaterThanZero")
                    .WithData("propertyName", "Pre-Money Valuation");
            }

            PreMoneyValuation = Math.Round(value, 2);
        }

        public decimal GetOwnershipPercentage(decimal investment)
        {
            if (PostMoneyValuation == 0)
                return 0;

            return (investment / PostMoneyValuation) * 100;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return PreMoneyValuation;
            yield return PostMoneyValuation;
            yield return Currency;
        }

        public override string ToString()
        {
            return $"Pre: {Currency} {PreMoneyValuation:N2}, Post: {Currency} {PostMoneyValuation:N2}";
        }
    }
}
