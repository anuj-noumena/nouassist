using System;
using System.Linq;
using System.Threading.Tasks;
using NouAssist.FinancialManagement.AccountsReceivable.Aggregates;
using NouAssist.FinancialManagement.AccountsReceivable.Data;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.FinancialManagement.AccountsReceivable.Services
{
    public class CreditManagementService : DomainService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IInvoiceRepository _invoiceRepository;

        public CreditManagementService(
            ICustomerRepository customerRepository,
            IInvoiceRepository invoiceRepository)
        {
            _customerRepository = customerRepository;
            _invoiceRepository = invoiceRepository;
        }

        public async Task<CreditDecisionResult> EvaluateCreditRequestAsync(
            Guid customerId,
            MonetaryValue requestedAmount)
        {
            var customer = await _customerRepository.GetAsync(customerId);
            var currentBalance = customer.CurrentBalance;
            var proposedBalance = currentBalance.Amount + requestedAmount.Amount;

            // Check credit limit
            if (proposedBalance > customer.CreditTerms.CreditLimit)
            {
                return CreditDecisionResult.Declined(
                    "Would exceed credit limit",
                    customer.CreditTerms.CreditLimit - currentBalance.Amount);
            }

            // Check if customer is active and not suspended
            if (!customer.IsActive || customer.IsCreditSuspended)
            {
                return CreditDecisionResult.Declined(
                    "Customer account is inactive or credit is suspended");
            }

            // Check payment history
            var invoices = await _invoiceRepository.GetByCustomerAsync(customerId);
            var paidInvoices = invoices.Where(i => i.Status == Enums.InvoiceStatus.Paid).ToList();

            if (paidInvoices.Any())
            {
                var averageDaysToPayment = paidInvoices.Average(i => 
                    (i.LastModificationTime.Value - i.IssueDate).Days);

                if (averageDaysToPayment > 45)
                {
                    return CreditDecisionResult.RequiresApproval(
                        "Customer has slow payment history");
                }
            }

            // Check for overdue invoices
            var overdueInvoices = invoices.Where(i => i.Status == Enums.InvoiceStatus.Overdue).ToList();
            if (overdueInvoices.Any())
            {
                var totalOverdue = overdueInvoices.Sum(i => i.Balance.Amount);
                if (totalOverdue > customer.CreditTerms.CreditLimit * 0.1m)
                {
                    return CreditDecisionResult.Declined(
                        "Customer has significant overdue balance");
                }
            }

            return CreditDecisionResult.Approved(requestedAmount);
        }
    }

    public class CreditDecisionResult
    {
        public bool IsApproved { get; }
        public bool RequiresApproval { get; }
        public string Reason { get; }
        public decimal AvailableCredit { get; }

        private CreditDecisionResult(bool isApproved, bool requiresApproval, string reason, decimal availableCredit = 0)
        {
            IsApproved = isApproved;
            RequiresApproval = requiresApproval;
            Reason = reason;
            AvailableCredit = availableCredit;
        }

        public static CreditDecisionResult Approved(MonetaryValue amount) =>
            new(true, false, "Credit approved", amount.Amount);

        public static CreditDecisionResult Declined(string reason, decimal availableCredit = 0) =>
            new(false, false, reason, availableCredit);

        public static CreditDecisionResult RequiresApproval(string reason) =>
            new(false, true, reason);
    }
}
