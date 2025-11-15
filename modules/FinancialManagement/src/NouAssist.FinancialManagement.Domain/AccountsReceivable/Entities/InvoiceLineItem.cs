using System;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.FinancialManagement.AccountsReceivable.Entities
{
    public class InvoiceLineItem : Entity<Guid>
    {
        public Guid InvoiceId { get; private set; }
        public string Description { get; private set; }
        public decimal Quantity { get; private set; }
        public MonetaryValue UnitPrice { get; private set; }
        public MonetaryValue SubTotal { get; private set; }
        public string Notes { get; private set; }
        public Guid? ProductId { get; private set; }
        public string TaxCode { get; private set; }

        protected InvoiceLineItem() { }

        public InvoiceLineItem(
            Guid id,
            string description,
            decimal quantity,
            MonetaryValue unitPrice,
            string notes = null,
            Guid? productId = null,
            string taxCode = null) : base(id)
        {
            Description = Check.NotNullOrWhiteSpace(description, nameof(description));
            
            if (quantity <= 0)
                throw new BusinessException("AR:InvalidQuantity", 
                    "Quantity must be greater than zero");
            
            Quantity = quantity;
            UnitPrice = Check.NotNull(unitPrice, nameof(unitPrice));
            SubTotal = unitPrice.Multiply(quantity);
            Notes = notes;
            ProductId = productId;
            TaxCode = taxCode;
        }

        public void UpdateQuantity(decimal newQuantity)
        {
            if (newQuantity <= 0)
                throw new BusinessException("AR:InvalidQuantity", 
                    "Quantity must be greater than zero");
            
            Quantity = newQuantity;
            SubTotal = UnitPrice.Multiply(newQuantity);
        }
    }
}
