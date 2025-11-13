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

        protected AccountingPeriod() { } // For ORM

        public AccountingPeriod(
            Guid id,
            Guid? tenantId,
            int fiscalYear,
            int periodNumber,
            DateTime startDate,
            DateTime endDate)
            : base(id)
        {
            if (periodNumber < 1 || periodNumber > 12)
                throw new ArgumentException("Period number must be between 1 and 12", 
                    nameof(periodNumber));

            if (endDate <= startDate)
                throw new ArgumentException("End date must be after start date");

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
                throw new BusinessException("GeneralLedger:PeriodAlreadyClosed",
                    "Period is already closed");

            Status = PeriodStatus.Closed;
            ClosedDate = DateTime.UtcNow;
            ClosedBy = closedBy;

            AddDistributedEvent(new AccountingPeriodClosedEto
            {
                IdempotencyKey = Guid.NewGuid().ToString(),
                PeriodId = Id,
                FiscalYear = FiscalYear,
                PeriodNumber = PeriodNumber,
                ClosedDate = ClosedDate.Value,
                ClosingEntries = new System.Collections.Generic.List<Guid>(),
                ClosedBy = closedBy,
                TenantId = TenantId
            });
        }

        public void Reopen(string reason)
        {
            if (Status == PeriodStatus.PermanentlyClosed)
                throw new BusinessException("GeneralLedger:PeriodPermanentlyClosed",
                    "Cannot reopen permanently closed period");

            if (Status != PeriodStatus.Closed)
                throw new BusinessException("GeneralLedger:PeriodNotClosed",
                    "Period is not closed");

            Status = PeriodStatus.Open;
            ClosedDate = null;
            ClosedBy = null;
        }
    }
}
