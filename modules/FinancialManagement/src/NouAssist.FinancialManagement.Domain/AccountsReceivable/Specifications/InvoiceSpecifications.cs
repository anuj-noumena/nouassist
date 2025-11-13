using System;
using System.Linq.Expressions;
using NouAssist.FinancialManagement.AccountsReceivable.Aggregates;
using NouAssist.FinancialManagement.AccountsReceivable.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.FinancialManagement.AccountsReceivable.Specifications
{
    public class OverdueInvoicesSpecification : Specification<Invoice>
    {
        public override Expression<Func<Invoice, bool>> ToExpression() =>
            invoice => invoice.Status == InvoiceStatus.Overdue;
    }

    public class UnpaidInvoicesByCustomerSpecification : Specification<Invoice>
    {
        private readonly Guid _customerId;

        public UnpaidInvoicesByCustomerSpecification(Guid customerId)
        {
            _customerId = customerId;
        }

        public override Expression<Func<Invoice, bool>> ToExpression() =>
            invoice => invoice.CustomerId == _customerId &&
                      (invoice.Status == InvoiceStatus.Sent ||
                       invoice.Status == InvoiceStatus.PartiallyPaid ||
                       invoice.Status == InvoiceStatus.Overdue);
    }

    public class InvoicesDueBeforeDateSpecification : Specification<Invoice>
    {
        private readonly DateTime _date;

        public InvoicesDueBeforeDateSpecification(DateTime date)
        {
            _date = date;
        }

        public override Expression<Func<Invoice, bool>> ToExpression() =>
            invoice => invoice.DueDate < _date &&
                      invoice.Status != InvoiceStatus.Paid &&
                      invoice.Status != InvoiceStatus.Cancelled;
    }
}
