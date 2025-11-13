using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.FinancialManagement.AccountsPayable.ValueObjects
{
    public class PaymentTerms : ValueObject
    {
        public int NetDays { get; }
        public int? DiscountDays { get; }
        public decimal? DiscountPercentage { get; }

        protected PaymentTerms() { }

        public PaymentTerms(int netDays, int? discountDays = null, decimal? discountPercentage = null)
        {
            if (netDays <= 0)
                throw new System.ArgumentException("Net days must be positive", nameof(netDays));

            if (discountDays.HasValue && discountDays.Value >= netDays)
                throw new System.ArgumentException("Discount days must be less than net days");

            if (discountPercentage.HasValue && (discountPercentage.Value < 0 || discountPercentage.Value > 100))
                throw new System.ArgumentException("Discount percentage must be between 0 and 100");

            NetDays = netDays;
            DiscountDays = discountDays;
            DiscountPercentage = discountPercentage;
        }

        public static PaymentTerms Net30 => new(30);
        public static PaymentTerms Net15 => new(15);
        public static PaymentTerms Net60 => new(60);
        public static PaymentTerms Net30Discount2Percent10Days => new(30, 10, 2m);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return NetDays;
            yield return DiscountDays;
            yield return DiscountPercentage;
        }

        public override string ToString()
        {
            if (DiscountDays.HasValue && DiscountPercentage.HasValue)
                return $"{DiscountPercentage:F1}% {DiscountDays} Days, Net {NetDays}";
            return $"Net {NetDays}";
        }
    }
}
