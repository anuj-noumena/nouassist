using System;
using NouAssist.FinancialManagement.DebtManagement.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.FinancialManagement.DebtManagement.Entities
{
    public class DebtPayment : Entity
    {
        public Guid DebtAccountId { get; private set; }
        public DateTime PaymentDate { get; private set; }
        public decimal PaymentAmount { get; private set; }
        public decimal PrincipalAmount { get; private set; }
        public decimal InterestAmount { get; private set; }
        public decimal RemainingBalance { get; private set; }
        public PaymentStatus Status { get; private set; }
        public string? TransactionReference { get; private set; }
        public string? Notes { get; set; }

        protected DebtPayment() { /* ORM */ }

        public DebtPayment(
            Guid debtAccountId,
            DateTime paymentDate,
            decimal paymentAmount,
            decimal principalAmount,
            decimal interestAmount,
            decimal remainingBalance,
            string? transactionReference = null)
        {
            DebtAccountId = debtAccountId;
            PaymentDate = paymentDate;
            PaymentAmount = Check.Positive(paymentAmount, nameof(paymentAmount));
            PrincipalAmount = principalAmount;
            InterestAmount = interestAmount;
            RemainingBalance = remainingBalance;
            Status = PaymentStatus.Completed;
            TransactionReference = transactionReference;
        }

        public void MarkAsFailed(string reason)
        {
            Status = PaymentStatus.Failed;
            Notes = reason;
        }

        public void Reverse(string reason)
        {
            if (Status != PaymentStatus.Completed)
            {
                throw new BusinessException("DebtManagement:CannotReverseNonCompletedPayment");
            }
            Status = PaymentStatus.Reversed;
            Notes = reason;
        }

        public override object[] GetKeys() => new object[] { DebtAccountId, PaymentDate };
    }
}
