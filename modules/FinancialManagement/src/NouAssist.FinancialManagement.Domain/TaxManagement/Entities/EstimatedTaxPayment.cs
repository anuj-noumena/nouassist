using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.TaxManagement.Entities
{
    public class EstimatedTaxPayment : Entity<Guid>
    {
        public Guid TaxReturnId { get; private set; }
        public int Quarter { get; private set; }
        public DateTime DueDate { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime? PaymentDate { get; private set; }
        public bool IsPaid { get; private set; }
        public string? ConfirmationNumber { get; private set; }

        protected EstimatedTaxPayment() { /* ORM */ }

        public EstimatedTaxPayment(
            Guid id,
            Guid taxReturnId,
            int quarter,
            DateTime dueDate,
            decimal amount)
            : base(id)
        {
            TaxReturnId = taxReturnId;
            Quarter = Check.Range(quarter, nameof(quarter), 1, 4);
            DueDate = dueDate;
            Amount = Check.Positive(amount, nameof(amount));
            IsPaid = false;
        }

        public void MarkAsPaid(DateTime paymentDate, string confirmationNumber)
        {
            if (IsPaid)
            {
                throw new BusinessException("TaxManagement:PaymentAlreadyRecorded");
            }

            PaymentDate = paymentDate;
            ConfirmationNumber = Check.NotNullOrWhiteSpace(confirmationNumber, nameof(confirmationNumber));
            IsPaid = true;
        }
    }
}
