using System;
using System.Linq.Expressions;
using NouAssist.FinancialManagement.AccountsReceivable.Aggregates;
using NouAssist.FinancialManagement.AccountsReceivable.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.FinancialManagement.AccountsReceivable.Specifications
{
    public class ActiveCustomersSpecification : Specification<Customer>
    {
        public override Expression<Func<Customer, bool>> ToExpression() =>
            c => c.IsActive;
    }

    public class CustomersByTypeSpecification : Specification<Customer>
    {
        private readonly CustomerType _type;

        public CustomersByTypeSpecification(CustomerType type)
        {
            _type = type;
        }

        public override Expression<Func<Customer, bool>> ToExpression() =>
            c => c.CustomerType == _type;
    }

    public class CustomersByNameSpecification : Specification<Customer>
    {
        private readonly string _searchTerm;

        public CustomersByNameSpecification(string searchTerm)
        {
            _searchTerm = searchTerm?.ToLower() ?? string.Empty;
        }

        public override Expression<Func<Customer, bool>> ToExpression() =>
            c => c.CustomerName.ToLower().Contains(_searchTerm);
    }

    public class HighValueCustomersSpecification : Specification<Customer>
    {
        private readonly decimal _creditLimitThreshold;

        public HighValueCustomersSpecification(decimal creditLimitThreshold)
        {
            _creditLimitThreshold = creditLimitThreshold;
        }

        public override Expression<Func<Customer, bool>> ToExpression() =>
            c => c.CreditLimit.Amount >= _creditLimitThreshold;
    }
}
