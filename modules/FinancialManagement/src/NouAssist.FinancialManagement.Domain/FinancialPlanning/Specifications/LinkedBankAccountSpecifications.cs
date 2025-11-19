using System;
using System.Linq.Expressions;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using NouAssist.FinancialManagement.FinancialPlanning.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.FinancialManagement.FinancialPlanning.Specifications
{
    public class ActiveBankAccountsSpecification : Specification<LinkedBankAccount>
    {
        public override Expression<Func<LinkedBankAccount, bool>> ToExpression() =>
            a => a.IsActive && a.ConnectionStatus == ConnectionStatus.Active;
    }

    public class AccountsByUserSpecification : Specification<LinkedBankAccount>
    {
        private readonly Guid _userId;

        public AccountsByUserSpecification(Guid userId)
        {
            _userId = userId;
        }

        public override Expression<Func<LinkedBankAccount, bool>> ToExpression() =>
            a => a.UserId == _userId;
    }

    public class AccountsByTypeSpecification : Specification<LinkedBankAccount>
    {
        private readonly BankAccountType _accountType;

        public AccountsByTypeSpecification(BankAccountType accountType)
        {
            _accountType = accountType;
        }

        public override Expression<Func<LinkedBankAccount, bool>> ToExpression() =>
            a => a.AccountType == _accountType;
    }

    public class DisconnectedAccountsSpecification : Specification<LinkedBankAccount>
    {
        public override Expression<Func<LinkedBankAccount, bool>> ToExpression() =>
            a => a.ConnectionStatus == ConnectionStatus.Disconnected ||
                 a.ConnectionStatus == ConnectionStatus.Error;
    }

    public class AccountsNeedingSyncSpecification : Specification<LinkedBankAccount>
    {
        private readonly int _hoursThreshold;

        public AccountsNeedingSyncSpecification(int hoursThreshold = 24)
        {
            _hoursThreshold = hoursThreshold;
        }

        public override Expression<Func<LinkedBankAccount, bool>> ToExpression()
        {
            var syncThreshold = DateTime.UtcNow.AddHours(-_hoursThreshold);
            return a => a.IsActive && a.LastSyncDate < syncThreshold;
        }
    }
}
