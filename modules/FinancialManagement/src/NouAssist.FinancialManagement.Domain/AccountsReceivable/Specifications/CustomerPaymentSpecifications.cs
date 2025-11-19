using System;
using System.Linq.Expressions;
using NouAssist.FinancialManagement.AccountsReceivable.Aggregates;
using NouAssist.FinancialManagement.AccountsReceivable.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.FinancialManagement.AccountsReceivable.Specifications
{
    public class PaymentsByCustomerSpecification : Specification<CustomerPayment>
    {
        private readonly Guid _customerId;

        public PaymentsByCustomerSpecification(Guid customerId)
        {
            _customerId = customerId;
        }

        public override Expression<Func<CustomerPayment, bool>> ToExpression() =>
            p => p.CustomerId == _customerId;
    }

    public class PaymentsBySourceSpecification : Specification<CustomerPayment>
    {
        private readonly PaymentSource _source;

        public PaymentsBySourceSpecification(PaymentSource source)
        {
            _source = source;
        }

        public override Expression<Func<CustomerPayment, bool>> ToExpression() =>
            p => p.PaymentSource == _source;
    }

    public class PaymentsInPeriodSpecification : Specification<CustomerPayment>
    {
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;

        public PaymentsInPeriodSpecification(DateTime startDate, DateTime endDate)
        {
            _startDate = startDate;
            _endDate = endDate;
        }

        public override Expression<Func<CustomerPayment, bool>> ToExpression() =>
            p => p.PaymentDate >= _startDate && p.PaymentDate <= _endDate;
    }

    public class LargePaymentsSpecification : Specification<CustomerPayment>
    {
        private readonly decimal _threshold;

        public LargePaymentsSpecification(decimal threshold)
        {
            _threshold = threshold;
        }

        public override Expression<Func<CustomerPayment, bool>> ToExpression() =>
            p => p.Amount.Amount >= _threshold;
    }
}
