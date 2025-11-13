using System;
using System.Collections.Generic;
using NouAssist.FinancialManagement.AccountsReceivable.Enums;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.Application.Dtos;

namespace NouAssist.FinancialManagement.AccountsReceivable.Dtos
{
    public class InvoiceDto : FullAuditedEntityDto<Guid>
    {
        public string InvoiceNumber { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public MonetaryValue TotalAmount { get; set; }
        public MonetaryValue PaidAmount { get; set; }
        public MonetaryValue Balance { get; set; }
        public InvoiceStatus Status { get; set; }
        public Guid? ProjectId { get; set; }
        public List<InvoiceLineItemDto> LineItems { get; set; }
        public int DaysOverdue => Status == InvoiceStatus.Overdue 
            ? (DateTime.Today - DueDate).Days 
            : 0;
    }

    public class InvoiceLineItemDto
    {
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public MonetaryValue UnitPrice { get; set; }
        public MonetaryValue Total { get; set; }
    }

    public class CreateInvoiceDto
    {
        public Guid CustomerId { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public Guid? ProjectId { get; set; }
        public string Notes { get; set; }
        public List<CreateInvoiceLineItemDto> LineItems { get; set; }
    }

    public class CreateInvoiceLineItemDto
    {
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string Currency { get; set; }
    }

    public class ApplyPaymentDto
    {
        public Guid PaymentId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }

    public class GetInvoiceListDto : PagedAndSortedResultRequestDto
    {
        public Guid? CustomerId { get; set; }
        public InvoiceStatus? Status { get; set; }
        public DateTime? IssueDateFrom { get; set; }
        public DateTime? IssueDateTo { get; set; }
        public bool? OverdueOnly { get; set; }
    }
}
