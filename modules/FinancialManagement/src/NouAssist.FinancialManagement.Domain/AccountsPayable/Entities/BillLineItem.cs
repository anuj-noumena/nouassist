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
        public MonetaryValue LineTotal { get; private set; }
        public Guid? ProductId { get; private set; }
        public string TaxCode { get; private set; }

        protected BillLineItem() { }

        public BillLineItem(
            Guid billId,
            string description,
            decimal quantity,
            MonetaryValue unitPrice,
            Guid? productId = null,
            string taxCode = null)
        {
            BillId = billId;
            Description = Check.NotNullOrWhiteSpace(description, nameof(description));
            
            if (quantity <= 0)
                throw new BusinessException("AP:InvalidQuantity", 
                    "Quantity must be greater than zero");
            
            Quantity = quantity;
            UnitPrice = Check.NotNull(unitPrice, nameof(unitPrice));
            LineTotal = unitPrice.Multiply(quantity);
            ProductId = productId;
            TaxCode = taxCode;
        }

        public override object[] GetKeys()
        {
            return new object[] { BillId, Description };
        }
    }
}
