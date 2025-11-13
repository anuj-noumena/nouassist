using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.FinancialManagement.AccountsReceivable.ValueObjects
{
    public class CreditTerms : ValueObject
    {
        public int DueDays { get; }
        public decimal CreditLimit { get; }
        public string Currency { get; }

        protected CreditTerms() { }

        public CreditTerms(int dueDays, decimal creditLimit, string currency = "USD")
        {
            if (dueDays <= 0)
                throw new System.ArgumentException("Due days must be positive", nameof(dueDays));

            if (creditLimit < 0)
                throw new System.ArgumentException("Credit limit cannot be negative", nameof(creditLimit));

            DueDays = dueDays;
            CreditLimit = creditLimit;
            Currency = currency;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return DueDays;
            yield return CreditLimit;
            yield return Currency;
        }

        public override string ToString() => $"Net {DueDays}, Limit: {CreditLimit:C} {Currency}";
    }
}
