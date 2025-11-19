using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.FinancialManagement.FinancialPlanning.Enums;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.FinancialPlanning.Aggregates
{
    public class ExpenseReport : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; private set; }
        public Guid SubmitterId { get; private set; }
        public string ReportName { get; private set; }
        public DateTime PeriodStart { get; private set; }
        public DateTime PeriodEnd { get; private set; }
        public ReportStatus Status { get; private set; }
        public MonetaryValue TotalAmount { get; private set; }
        public Guid? ApprovedBy { get; private set; }
        public DateTime? SubmittedDate { get; private set; }

        private readonly List<Guid> _expenseIds = new();
        public IReadOnlyCollection<Guid> ExpenseIds => _expenseIds.AsReadOnly();

        protected ExpenseReport() { }

        public ExpenseReport(
            Guid id,
            Guid? tenantId,
            Guid submitterId,
            string reportName,
            DateTime periodStart,
            DateTime periodEnd,
            string currency) : base(id)
        {
            TenantId = tenantId;
            SubmitterId = submitterId;
            ReportName = Check.NotNullOrWhiteSpace(reportName, nameof(reportName));
            PeriodStart = periodStart;
            PeriodEnd = periodEnd;
            Status = ReportStatus.Draft;
            TotalAmount = MonetaryValue.Zero(currency);
        }

        public void AddExpense(Guid expenseId)
        {
            if (Status != ReportStatus.Draft)
            {
                throw new BusinessException("FPA:CannotModifySubmittedReport",
                    "Cannot add expenses to submitted report");
            }

            if (_expenseIds.Contains(expenseId))
            {
                throw new BusinessException("FPA:ExpenseAlreadyAdded",
                    "Expense already added to report");
            }

            _expenseIds.Add(expenseId);
        }

        public void Submit()
        {
            if (Status != ReportStatus.Draft)
            {
                throw new BusinessException("FPA:ReportAlreadySubmitted",
                    "Report has already been submitted");
            }

            if (!_expenseIds.Any())
            {
                throw new BusinessException("FPA:NoExpenses",
                    "Report must contain at least one expense");
            }

            Status = ReportStatus.Submitted;
            SubmittedDate = DateTime.UtcNow;
        }

        public void Approve(Guid approvedBy)
        {
            if (Status != ReportStatus.Submitted)
            {
                throw new BusinessException("FPA:NotSubmitted",
                    "Only submitted reports can be approved");
            }

            Status = ReportStatus.Approved;
            ApprovedBy = approvedBy;
        }

        public void Reject(string reason)
        {
            if (Status != ReportStatus.Submitted)
            {
                throw new BusinessException("FPA:NotSubmitted",
                    "Only submitted reports can be rejected");
            }

            Status = ReportStatus.Rejected;
        }

        public void UpdateTotalAmount(MonetaryValue totalAmount)
        {
            TotalAmount = totalAmount;
        }
    }
}
