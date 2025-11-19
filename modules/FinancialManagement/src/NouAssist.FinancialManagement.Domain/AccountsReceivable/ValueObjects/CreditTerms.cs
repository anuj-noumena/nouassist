using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.FinancialManagement.AccountsReceivable.ValueObjects
{
    public class CreditTerms : ValueObject
    {
        public int PaymentTermsDays { get; private set; }
        public decimal CreditLimit { get; private set; }
        public string Currency { get; private set; }
        public bool RequiresApproval { get; private set; }

        private CreditTerms() { }

        public CreditTerms(int paymentTermsDays, decimal creditLimit, string currency, bool requiresApproval = false)
        {
            PaymentTermsDays = paymentTermsDays;
            CreditLimit = creditLimit;
            Currency = currency;
            RequiresApproval = requiresApproval;
        }

        public static CreditTerms Standard(string currency) => new CreditTerms(30, 10000m, currency);
        public static CreditTerms Premium(string currency) => new CreditTerms(60, 50000m, currency);
        public static CreditTerms CashOnly(string currency) => new CreditTerms(0, 0m, currency, true);

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return PaymentTermsDays;
            yield return CreditLimit;
            yield return Currency;
            yield return RequiresApproval;
        }
    }
}
