using System;
using NouAssist.FinancialManagement.AccountsReceivable.Enums;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.Application.Dtos;

namespace NouAssist.FinancialManagement.AccountsReceivable.Dtos
{
    public class CreditMemoDto : FullAuditedEntityDto<Guid>
    {
        public string CreditMemoNumber { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public MonetaryValue Amount { get; set; }
        public DateTime IssueDate { get; set; }
        public CreditMemoReason Reason { get; set; }
        public string Description { get; set; }
        public Guid? OriginalInvoiceId { get; set; }
        public bool IsApplied { get; set; }
    }

    public class CreateCreditMemoDto
    {
        public Guid CustomerId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime IssueDate { get; set; }
        public CreditMemoReason Reason { get; set; }
        public string Description { get; set; }
        public Guid? OriginalInvoiceId { get; set; }
    }
}
