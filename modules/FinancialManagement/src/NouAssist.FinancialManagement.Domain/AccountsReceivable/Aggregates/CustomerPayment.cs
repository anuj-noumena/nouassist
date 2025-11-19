using System;
using NouAssist.FinancialManagement.AccountsReceivable.Enums;
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
        public DateTime PaymentDate { get; private set; }
        public string PaymentMethod { get; private set; }
        public string Reference { get; private set; }
        public PaymentSource PaymentSource { get; private set; }

        protected CustomerPayment() { }

        public CustomerPayment(
            Guid id,
            Guid? tenantId,
            Guid customerId,
            MonetaryValue amount,
            DateTime paymentDate,
            string paymentMethod,
            string reference,
            PaymentSource source) : base(id)
        {
            TenantId = tenantId;
            CustomerId = customerId;
            Amount = Check.NotNull(amount, nameof(amount));
            PaymentDate = paymentDate;
            PaymentMethod = Check.NotNullOrWhiteSpace(paymentMethod, nameof(paymentMethod));
            Reference = Check.NotNullOrWhiteSpace(reference, nameof(reference));
            PaymentSource = source;
        }
    }
}
