using System;
using System.Collections.Generic;
using NouAssist.FinancialManagement.AccountsReceivable.Entities;
using NouAssist.FinancialManagement.AccountsReceivable.Enums;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.AccountsReceivable.Aggregates
{
    public class CollectionCase : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; private set; }
        public Guid CustomerId { get; private set; }
        public MonetaryValue TotalOverdueAmount { get; private set; }
        public CollectionStatus Status { get; private set; }
        public Guid AssignedTo { get; private set; }
        public DateTime? LastContactDate { get; private set; }

        private readonly List<Guid> _overdueInvoices = new();
        public IReadOnlyCollection<Guid> OverdueInvoices => _overdueInvoices.AsReadOnly();

        private readonly List<CollectionActivity> _activities = new();
        public IReadOnlyCollection<CollectionActivity> Activities => _activities.AsReadOnly();

        protected CollectionCase() { } // For ORM

        public CollectionCase(
            Guid id,
            Guid? tenantId,
            Guid customerId,
            MonetaryValue totalOverdueAmount,
            Guid assignedTo)
            : base(id)
        {
            TenantId = tenantId;
            CustomerId = customerId;
            TotalOverdueAmount = Check.NotNull(totalOverdueAmount, nameof(totalOverdueAmount));
            Status = CollectionStatus.Open;
            AssignedTo = assignedTo;
        }

        public void AddOverdueInvoice(Guid invoiceId)
        {
            if (_overdueInvoices.Contains(invoiceId))
                throw new BusinessException("AccountsReceivable:InvoiceAlreadyInCase",
                    "Invoice already added to collection case");

            _overdueInvoices.Add(invoiceId);
        }

        public void LogActivity(CollectionActivity activity)
        {
            _activities.Add(activity);
            LastContactDate = activity.ActivityDate;

            if (Status == CollectionStatus.Open)
                Status = CollectionStatus.InProgress;
        }

        public void Escalate(string reason)
        {
            if (Status == CollectionStatus.Legal)
                throw new BusinessException("AccountsReceivable:AlreadyEscalated",
                    "Case has already been escalated to legal");

            Status = CollectionStatus.Legal;
        }

        public void Resolve(string resolution)
        {
            if (Status == CollectionStatus.Resolved)
                throw new BusinessException("AccountsReceivable:AlreadyResolved",
                    "Case has already been resolved");

            Status = CollectionStatus.Resolved;
        }

        public void PlaceOnHold(string reason)
        {
            Status = CollectionStatus.OnHold;
        }

        public void Resume()
        {
            if (Status != CollectionStatus.OnHold)
                throw new BusinessException("AccountsReceivable:CaseNotOnHold",
                    "Case is not on hold");

            Status = CollectionStatus.InProgress;
        }
    }
}
