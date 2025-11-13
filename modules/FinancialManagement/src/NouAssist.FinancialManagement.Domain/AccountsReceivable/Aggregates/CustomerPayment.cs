using System;
using System.Collections.Generic;
using NouAssist.FinancialManagement.AccountsReceivable.Enums;
using NouAssist.FinancialManagement.AccountsReceivable.Events;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.AccountsReceivable.Aggregates
{
    public class CustomerPayment : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; private set; }
        public Guid CustomerId { get; private set; }
        public MonetaryValue Amount { get; private set; }
        public MonetaryValue UnappliedAmount { get; private set; }
        public DateTime PaymentDate { get; private set; }
        public string PaymentMethod { get; private set; }
        public string Reference { get; private set; }
        public PaymentSource Source { get; private set; }
        public bool IsFullyApplied { get; private set; }

        protected CustomerPayment() { } // For ORM

        public CustomerPayment(
            Guid id,
            Guid? tenantId,
            Guid customerId,
            MonetaryValue amount,
            DateTime paymentDate,
            string paymentMethod,
            string reference,
            PaymentSource source)
            : base(id)
        {
            if (amount.IsNegative || amount.IsZero)
                throw new BusinessException("AccountsReceivable:InvalidPaymentAmount",
                    "Payment amount must be positive");

            TenantId = tenantId;
            CustomerId = customerId;
            Amount = amount;
            UnappliedAmount = amount;
            PaymentDate = paymentDate;
            PaymentMethod = Check.NotNullOrWhiteSpace(paymentMethod, nameof(paymentMethod), 50);
            Reference = Check.NotNullOrWhiteSpace(reference, nameof(reference), 100);
            Source = source;
            IsFullyApplied = false;

            AddDistributedEvent(new PaymentReceivedEto
            {
                IdempotencyKey = Guid.NewGuid().ToString(),
                PaymentId = Id,
                CustomerId = CustomerId,
                Amount = Amount,
                PaymentDate = PaymentDate,
                PaymentMethod = PaymentMethod,
                PaymentSource = Source.ToString(),
                Reference = Reference,
                RecordedBy = CreatorId.Value,
                TenantId = TenantId
            });
        }

        public void ApplyToInvoice(MonetaryValue appliedAmount)
        {
            if (appliedAmount.Amount > UnappliedAmount.Amount)
                throw new BusinessException("AccountsReceivable:InsufficientUnappliedAmount",
                    "Applied amount exceeds unapplied payment amount");

            UnappliedAmount = UnappliedAmount.Subtract(appliedAmount);
            IsFullyApplied = UnappliedAmount.IsZero;
        }

        public void UnapplyFromInvoice(MonetaryValue unappliedAmount)
        {
            UnappliedAmount = UnappliedAmount.Add(unappliedAmount);
            IsFullyApplied = false;
        }
    }
}
