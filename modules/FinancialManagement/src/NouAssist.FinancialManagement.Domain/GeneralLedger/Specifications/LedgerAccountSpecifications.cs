using System;
using System.Linq.Expressions;
using NouAssist.FinancialManagement.GeneralLedger.Aggregates;
using NouAssist.FinancialManagement.GeneralLedger.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.FinancialManagement.GeneralLedger.Specifications
{
    public class ActiveAccountsSpecification : Specification<LedgerAccount>
    {
        public override Expression<Func<LedgerAccount, bool>> ToExpression()
        {
            return account => account.IsActive;
        }
    }

    public class AccountsByTypeSpecification : Specification<LedgerAccount>
    {
        private readonly AccountType _accountType;

        public AccountsByTypeSpecification(AccountType accountType)
        {
            _accountType = accountType;
        }

        public override Expression<Func<LedgerAccount, bool>> ToExpression()
        {
            return account => account.Type == _accountType;
        }
    }

    public class PostableAccountsSpecification : Specification<LedgerAccount>
    {
        public override Expression<Func<LedgerAccount, bool>> ToExpression()
        {
            return account => account.IsActive && account.AllowsPosting;
        }
    }
}
