using System;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.Domain.Entities;

namespace NouAssist.FinancialManagement.AccountsReceivable.Entities
{
    public class InvoicePaymentApplication : Entity
    {
        public Guid InvoiceId { get; private set; }
        public Guid PaymentId { get; private set; }
        public MonetaryValue AppliedAmount { get; private set; }
        public DateTime ApplicationDate { get; private set; }
        public bool IsReversed { get; private set; }

        protected InvoicePaymentApplication() { }

        public InvoicePaymentApplication(
            Guid invoiceId,
            Guid paymentId,
            MonetaryValue appliedAmount,
            DateTime applicationDate)
        {
            InvoiceId = invoiceId;
            PaymentId = paymentId;
            AppliedAmount = appliedAmount;
            ApplicationDate = applicationDate;
            IsReversed = false;
        }

        public void Reverse()
        {
            if (IsReversed)
                throw new InvalidOperationException("Payment application already reversed");

            IsReversed = true;
        }

        public override object[] GetKeys()
        {
            return new object[] { InvoiceId, PaymentId };
        }
    }
}
