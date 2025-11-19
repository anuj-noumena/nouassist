using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.FinancialManagement.AccountsPayable.ValueObjects
{
    public class PaymentTerms : ValueObject
    {
        public int NetDays { get; private set; }
        public int? DiscountDays { get; private set; }
        public decimal? DiscountPercentage { get; private set; }

        private PaymentTerms() { }

        public PaymentTerms(int netDays, int? discountDays = null, decimal? discountPercentage = null)
        {
            NetDays = netDays;
            DiscountDays = discountDays;
            DiscountPercentage = discountPercentage;
        }

        public static PaymentTerms Net30() => new PaymentTerms(30);
        public static PaymentTerms Net60() => new PaymentTerms(60);
        public static PaymentTerms Net90() => new PaymentTerms(90);
        public static PaymentTerms TwoTenNet30() => new PaymentTerms(30, 10, 2.0m);

        public string GetDescription()
        {
            if (DiscountDays.HasValue && DiscountPercentage.HasValue)
            {
                return $"{DiscountPercentage}% {DiscountDays} Net {NetDays}";
            }
            return $"Net {NetDays}";
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return NetDays;
            yield return DiscountDays ?? 0;
            yield return DiscountPercentage ?? 0;
        }
    }
}
