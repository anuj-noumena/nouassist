using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.FinancialManagement.AccountsReceivable.Aggregates;
using NouAssist.FinancialManagement.AccountsReceivable.Enums;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace NouAssist.FinancialManagement.AccountsReceivable.Services
{
    public class PaymentAllocationService : DomainService
    {
        public List<PaymentAllocation> AllocatePaymentToInvoices(
            CustomerPayment payment,
            List<Invoice> outstandingInvoices)
        {
            var allocations = new List<PaymentAllocation>();
            var remainingAmount = payment.Amount.Amount;

            // Sort invoices by due date (oldest first)
            var sortedInvoices = outstandingInvoices
                .Where(i => i.Status == InvoiceStatus.Sent || i.Status == InvoiceStatus.Overdue)
                .OrderBy(i => i.DueDate)
                .ToList();

            foreach (var invoice in sortedInvoices)
            {
                if (remainingAmount <= 0)
                    break;

                var invoiceBalance = invoice.TotalAmount.Amount; // Simplified - should track remaining balance
                var allocationAmount = Math.Min(remainingAmount, invoiceBalance);

                allocations.Add(new PaymentAllocation(
                    payment.Id,
                    invoice.Id,
                    new MonetaryValue(allocationAmount, payment.Amount.Currency),
                    Clock.Now
                ));

                remainingAmount -= allocationAmount;

                // Update invoice status if fully paid
                if (allocationAmount >= invoiceBalance)
                {
                    invoice.MarkAsPaid(Clock.Now);
                }
            }

            // Check for unapplied payment
            if (remainingAmount > 0)
            {
                allocations.Add(new PaymentAllocation(
                    payment.Id,
                    null, // No invoice - unapplied payment
                    new MonetaryValue(remainingAmount, payment.Amount.Currency),
                    Clock.Now
                ));
            }

            return allocations;
        }

        public PaymentAllocation AllocatePaymentToSpecificInvoice(
            CustomerPayment payment,
            Invoice invoice,
            decimal? specifiedAmount = null)
        {
            if (invoice.Status == InvoiceStatus.Paid)
            {
                throw new BusinessException("AccountsReceivable:InvoiceAlreadyPaid",
                    "Cannot allocate payment to an invoice that is already paid");
            }

            var invoiceBalance = invoice.TotalAmount.Amount; // Simplified
            var allocationAmount = specifiedAmount ?? Math.Min(payment.Amount.Amount, invoiceBalance);

            if (allocationAmount > payment.Amount.Amount)
            {
                throw new BusinessException("AccountsReceivable:AllocationExceedsPayment",
                    "Allocation amount exceeds payment amount");
            }

            var allocation = new PaymentAllocation(
                payment.Id,
                invoice.Id,
                new MonetaryValue(allocationAmount, payment.Amount.Currency),
                Clock.Now
            );

            if (allocationAmount >= invoiceBalance)
            {
                invoice.MarkAsPaid(Clock.Now);
            }

            return allocation;
        }

        public decimal CalculateTotalAllocated(List<PaymentAllocation> allocations)
        {
            return allocations.Sum(a => a.Amount.Amount);
        }

        public decimal CalculateUnappliedAmount(CustomerPayment payment, List<PaymentAllocation> allocations)
        {
            var totalAllocated = CalculateTotalAllocated(allocations);
            return payment.Amount.Amount - totalAllocated;
        }
    }

    public class PaymentAllocation
    {
        public Guid PaymentId { get; }
        public Guid? InvoiceId { get; }
        public MonetaryValue Amount { get; }
        public DateTime AllocationDate { get; }
        public bool IsUnapplied => !InvoiceId.HasValue;

        public PaymentAllocation(
            Guid paymentId,
            Guid? invoiceId,
            MonetaryValue amount,
            DateTime allocationDate)
        {
            PaymentId = paymentId;
            InvoiceId = invoiceId;
            Amount = amount;
            AllocationDate = allocationDate;
        }
    }
}
