using System;
using System.Linq.Expressions;
using NouAssist.FinancialManagement.AccountsReceivable.Aggregates;
using NouAssist.FinancialManagement.AccountsReceivable.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.FinancialManagement.AccountsReceivable.Specifications
{
    public class UnpaidInvoicesSpecification : Specification<Invoice>
    {
        public override Expression<Func<Invoice, bool>> ToExpression() =>
            i => i.Status == InvoiceStatus.Sent || i.Status == InvoiceStatus.Overdue;
    }

    public class OverdueInvoicesSpecification : Specification<Invoice>
    {
        public override Expression<Func<Invoice, bool>> ToExpression() =>
            i => (i.Status == InvoiceStatus.Sent || i.Status == InvoiceStatus.Overdue)
                && i.DueDate < DateTime.UtcNow;
    }

    public class InvoicesByCustomerSpecification : Specification<Invoice>
    {
        private readonly Guid _customerId;

        public InvoicesByCustomerSpecification(Guid customerId)
        {
            _customerId = customerId;
        }

        public override Expression<Func<Invoice, bool>> ToExpression() =>
            i => i.CustomerId == _customerId;
    }

    public class InvoicesDueInPeriodSpecification : Specification<Invoice>
    {
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;

        public InvoicesDueInPeriodSpecification(DateTime startDate, DateTime endDate)
        {
            _startDate = startDate;
            _endDate = endDate;
        }

        public override Expression<Func<Invoice, bool>> ToExpression() =>
            i => i.DueDate >= _startDate && i.DueDate <= _endDate;
    }

    public class DraftInvoicesSpecification : Specification<Invoice>
    {
        public override Expression<Func<Invoice, bool>> ToExpression() =>
            i => i.Status == InvoiceStatus.Draft;
    }

    public class HighValueInvoicesSpecification : Specification<Invoice>
    {
        private readonly decimal _threshold;

        public HighValueInvoicesSpecification(decimal threshold)
        {
            _threshold = threshold;
        }

        public override Expression<Func<Invoice, bool>> ToExpression() =>
            i => i.TotalAmount.Amount >= _threshold;
    }
}
