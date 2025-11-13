using System;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.FinancialManagement.AccountsReceivable.Entities
{
    public class InvoiceLineItem : Entity
    {
        public Guid InvoiceId { get; private set; }
        public string Description { get; private set; }
        public decimal Quantity { get; private set; }
        public MonetaryValue UnitPrice { get; private set; }
        public MonetaryValue Total { get; private set; }
        public Guid? ProductId { get; private set; }
        public Guid? ServiceId { get; private set; }

        protected InvoiceLineItem() { }

        public InvoiceLineItem(
            Guid invoiceId,
            string description,
            decimal quantity,
            MonetaryValue unitPrice,
            Guid? productId = null,
            Guid? serviceId = null)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be positive", nameof(quantity));

            InvoiceId = invoiceId;
            Description = Check.NotNullOrWhiteSpace(description, nameof(description), 500);
            Quantity = quantity;
            UnitPrice = Check.NotNull(unitPrice, nameof(unitPrice));
            ProductId = productId;
            ServiceId = serviceId;
            
            Total = unitPrice.Multiply(quantity);
        }

        public override object[] GetKeys()
        {
            return new object[] { InvoiceId, Description };
        }
    }
}
