using System;
using System.Collections.Generic;
using NouAssist.FinancialManagement.AccountsReceivable.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.FinancialManagement.Application.Contracts.AccountsReceivable.Dtos
{
    public class InvoiceDto : FullAuditedEntityDto<Guid>
    {
        public string InvoiceNumber { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public InvoiceStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingBalance { get; set; }
        public string Currency { get; set; }
        public int DaysUntilDue { get; set; }
        public bool IsOverdue { get; set; }
        public List<InvoiceLineItemDto> LineItems { get; set; }
    }

    public class InvoiceLineItemDto
    {
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }
        public string Currency { get; set; }
    }

    public class CreateInvoiceDto
    {
        public string InvoiceNumber { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Currency { get; set; } = "USD";
        public Guid? ProjectId { get; set; }
        public List<CreateInvoiceLineItemDto> LineItems { get; set; }
    }

    public class CreateInvoiceLineItemDto
    {
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public Guid? ProductId { get; set; }
    }

    public class SendInvoiceDto
    {
        public Guid InvoiceId { get; set; }
    }

    public class ApplyPaymentToInvoiceDto
    {
        public Guid InvoiceId { get; set; }
        public decimal PaymentAmount { get; set; }
        public string Currency { get; set; } = "USD";
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public string Reference { get; set; }
    }

    public class GetInvoiceListDto : PagedAndSortedResultRequestDto
    {
        public Guid? CustomerFilter { get; set; }
        public InvoiceStatus? StatusFilter { get; set; }
        public DateTime? IssueDateFrom { get; set; }
        public DateTime? IssueDateTo { get; set; }
        public bool? OverdueOnly { get; set; }
    }
}
