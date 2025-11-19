using System;
using NouAssist.FinancialManagement.GeneralLedger.Enums;
using NouAssist.FinancialManagement.GeneralLedger.Events;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.GeneralLedger.Aggregates
{
    public class AccountingPeriod : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; private set; }
        public int FiscalYear { get; private set; }
        public int PeriodNumber { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public PeriodStatus Status { get; private set; }
        public DateTime? ClosedDate { get; private set; }
        public Guid? ClosedBy { get; private set; }

        protected AccountingPeriod() { }

        public AccountingPeriod(
            Guid id,
            Guid? tenantId,
            int fiscalYear,
            int periodNumber,
            DateTime startDate,
            DateTime endDate) : base(id)
        {
            if (periodNumber < 1 || periodNumber > 12)
            {
                throw new BusinessException("GL:InvalidPeriodNumber", 
                    "Period number must be between 1 and 12");
            }

            if (endDate <= startDate)
            {
                throw new BusinessException("GL:InvalidPeriodDates", 
                    "End date must be after start date");
            }

            TenantId = tenantId;
            FiscalYear = fiscalYear;
            PeriodNumber = periodNumber;
            StartDate = startDate;
            EndDate = endDate;
            Status = PeriodStatus.Open;
        }

        public void Close(Guid closedBy)
        {
            if (Status != PeriodStatus.Open)
            {
                throw new BusinessException("GL:PeriodNotOpen", 
                    "Only open periods can be closed");
            }

            Status = PeriodStatus.Closed;
            ClosedDate = DateTime.UtcNow;
            ClosedBy = closedBy;

            AddDistributedEvent(new AccountingPeriodClosedEto
            {
                IdempotencyKey = Guid.NewGuid().ToString(),
                PeriodId = Id,
                TenantId = TenantId,
                FiscalYear = FiscalYear,
                PeriodNumber = PeriodNumber,
                ClosedDate = ClosedDate.Value,
                ClosingEntries = new System.Collections.Generic.List<Guid>(),
                ClosedBy = closedBy
            });
        }

        public void Reopen(string reopenReason)
        {
            if (Status != PeriodStatus.Closed)
            {
                throw new BusinessException("GL:PeriodNotClosed", 
                    "Only closed periods can be reopened");
            }

            if (Status == PeriodStatus.PermanentlyClosed)
            {
                throw new BusinessException("GL:PeriodPermanentlyClosed", 
                    "Permanently closed periods cannot be reopened");
            }

            Status = PeriodStatus.Open;
            ClosedDate = null;
            ClosedBy = null;
        }

        public void PermanentlyClose()
        {
            if (Status != PeriodStatus.Closed)
            {
                throw new BusinessException("GL:MustBeClosedFirst", 
                    "Period must be closed before permanent closure");
            }

            Status = PeriodStatus.PermanentlyClosed;
        }

        public bool IsDateInPeriod(DateTime date)
        {
            return date.Date >= StartDate.Date && date.Date <= EndDate.Date;
        }
    }
}
