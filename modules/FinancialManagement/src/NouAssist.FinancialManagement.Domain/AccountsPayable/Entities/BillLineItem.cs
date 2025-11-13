using System;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.FinancialManagement.AccountsPayable.Entities
{
    public class BillLineItem : Entity
    {
        public Guid BillId { get; private set; }
        public string Description { get; private set; }
        public decimal Quantity { get; private set; }
        public MonetaryValue UnitPrice { get; private set; }
        public MonetaryValue Total { get; private set; }
        public Guid? ProductId { get; private set; }

        protected BillLineItem() { }

        public BillLineItem(
            Guid billId,
            string description,
            decimal quantity,
            MonetaryValue unitPrice,
            Guid? productId = null)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be positive", nameof(quantity));

            BillId = billId;
            Description = Check.NotNullOrWhiteSpace(description, nameof(description), 500);
            Quantity = quantity;
            UnitPrice = Check.NotNull(unitPrice, nameof(unitPrice));
            ProductId = productId;
            
            Total = unitPrice.Multiply(quantity);
        }

        public override object[] GetKeys()
        {
            return new object[] { BillId, Description };
        }
    }
}
