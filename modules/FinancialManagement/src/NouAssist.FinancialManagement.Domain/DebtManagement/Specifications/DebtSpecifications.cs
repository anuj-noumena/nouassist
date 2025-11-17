using System;
using System.Linq.Expressions;
using NouAssist.DebtManagement.Enums;
using NouAssist.FinancialManagement.DebtManagement.Aggregates;
using Volo.Abp.Specifications;

namespace NouAssist.FinancialManagement.DebtManagement.Specifications
{
    public class ActiveDebtsSpecification : Specification<DebtAccount>
    {
        public override Expression<Func<DebtAccount, bool>> ToExpression() =>
            d => d.Status == DebtStatus.Active;
    }

    public class HighInterestDebtsSpecification : Specification<DebtAccount>
    {
        private readonly decimal _threshold;

        public HighInterestDebtsSpecification(decimal interestRateThreshold = 15.0m)
        {
            _threshold = interestRateThreshold;
        }

        public override Expression<Func<DebtAccount, bool>> ToExpression() =>
            d => d.InterestRate >= _threshold && d.Status == DebtStatus.Active;
    }

    public class OverduePaymentsSpecification : Specification<DebtAccount>
    {
        public override Expression<Func<DebtAccount, bool>> ToExpression() =>
            d => d.Status == DebtStatus.Active &&
                 d.NextPaymentDueDate.HasValue &&
                 d.NextPaymentDueDate.Value < DateTime.UtcNow;
    }
}
