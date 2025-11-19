using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.FinancialManagement.GeneralLedger.ValueObjects
{
    public class AccountRange : ValueObject
    {
        public string StartAccount { get; private set; }
        public string EndAccount { get; private set; }

        private AccountRange() { }

        public AccountRange(string startAccount, string endAccount)
        {
            StartAccount = Check.NotNullOrWhiteSpace(startAccount, nameof(startAccount));
            EndAccount = Check.NotNullOrWhiteSpace(endAccount, nameof(endAccount));

            if (string.Compare(startAccount, endAccount, System.StringComparison.Ordinal) > 0)
            {
                throw new BusinessException("GeneralLedger:InvalidRange",
                    "Start account must be less than or equal to end account");
            }
        }

        public bool Contains(string accountNumber)
        {
            return string.Compare(accountNumber, StartAccount, System.StringComparison.Ordinal) >= 0 &&
                   string.Compare(accountNumber, EndAccount, System.StringComparison.Ordinal) <= 0;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return StartAccount;
            yield return EndAccount;
        }
    }
}
