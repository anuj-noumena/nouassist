using System;
using System.Linq.Expressions;
using NouAssist.FinancialManagement.AccountsPayable.Aggregates;
using NouAssist.FinancialManagement.AccountsPayable.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.FinancialManagement.AccountsPayable.Specifications
{
    public class PendingApprovalBillsSpecification : Specification<Bill>
    {
        public override Expression<Func<Bill, bool>> ToExpression() =>
            b => b.Status == BillStatus.PendingApproval;
    }

    public class ApprovedBillsSpecification : Specification<Bill>
    {
        public override Expression<Func<Bill, bool>> ToExpression() =>
            b => b.Status == BillStatus.Approved;
    }

    public class OverdueBillsSpecification : Specification<Bill>
    {
        public override Expression<Func<Bill, bool>> ToExpression() =>
            b => b.Status != BillStatus.Paid && b.DueDate < DateTime.UtcNow;
    }

    public class BillsDueInPeriodSpecification : Specification<Bill>
    {
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;

        public BillsDueInPeriodSpecification(DateTime startDate, DateTime endDate)
        {
            _startDate = startDate;
            _endDate = endDate;
        }

        public override Expression<Func<Bill, bool>> ToExpression() =>
            b => b.DueDate >= _startDate && b.DueDate <= _endDate;
    }

    public class BillsByVendorSpecification : Specification<Bill>
    {
        private readonly Guid _vendorId;

        public BillsByVendorSpecification(Guid vendorId)
        {
            _vendorId = vendorId;
        }

        public override Expression<Func<Bill, bool>> ToExpression() =>
            b => b.VendorId == _vendorId;
    }

    public class HighValueBillsSpecification : Specification<Bill>
    {
        private readonly decimal _threshold;

        public HighValueBillsSpecification(decimal threshold)
        {
            _threshold = threshold;
        }

        public override Expression<Func<Bill, bool>> ToExpression() =>
            b => b.TotalAmount.Amount >= _threshold;
    }
}
