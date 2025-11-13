using System;
using System.Linq.Expressions;
using NouAssist.FinancialManagement.GeneralLedger.Aggregates;
using NouAssist.FinancialManagement.GeneralLedger.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.FinancialManagement.GeneralLedger.Specifications
{
    public class ActiveAccountsSpecification : Specification<LedgerAccount>
    {
        public override Expression<Func<LedgerAccount, bool>> ToExpression() =>
            account => account.IsActive;
    }

    public class AccountsByTypeSpecification : Specification<LedgerAccount>
    {
        private readonly AccountType _type;

        public AccountsByTypeSpecification(AccountType type)
        {
            _type = type;
        }

        public override Expression<Func<LedgerAccount, bool>> ToExpression() =>
            account => account.Type == _type;
    }

    public class PostableAccountsSpecification : Specification<LedgerAccount>
    {
        public override Expression<Func<LedgerAccount, bool>> ToExpression() =>
            account => account.AllowsPosting && account.IsActive;
    }
}
