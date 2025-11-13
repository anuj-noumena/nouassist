using System;
using System.Collections.Generic;
using NouAssist.FinancialManagement.AccountsPayable.Enums;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.Application.Dtos;

namespace NouAssist.FinancialManagement.AccountsPayable.Dtos
{
    public class BillDto : FullAuditedEntityDto<Guid>
    {
        public string BillNumber { get; set; }
        public Guid VendorId { get; set; }
        public string VendorName { get; set; }
        public MonetaryValue Amount { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public BillStatus Status { get; set; }
        public Guid CategoryId { get; set; }
        public Guid? ProjectId { get; set; }
        public bool RequiresApproval { get; set; }
        public List<BillLineItemDto> LineItems { get; set; }
        public List<BillApprovalDto> Approvals { get; set; }
    }

    public class BillLineItemDto
    {
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public MonetaryValue UnitPrice { get; set; }
        public MonetaryValue Total { get; set; }
    }

    public class BillApprovalDto
    {
        public Guid ApproverId { get; set; }
        public ApprovalLevel Level { get; set; }
        public bool IsApproved { get; set; }
        public DateTime ApprovedDate { get; set; }
        public string Comments { get; set; }
    }

    public class CreateBillDto
    {
        public string BillNumber { get; set; }
        public Guid VendorId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public Guid CategoryId { get; set; }
        public Guid? ProjectId { get; set; }
        public string Notes { get; set; }
        public List<CreateBillLineItemDto> LineItems { get; set; }
    }

    public class CreateBillLineItemDto
    {
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string Currency { get; set; }
    }

    public class ApproveBillDto
    {
        public bool IsApproved { get; set; }
        public string Comments { get; set; }
    }

    public class GetBillListDto : PagedAndSortedResultRequestDto
    {
        public Guid? VendorId { get; set; }
        public BillStatus? Status { get; set; }
        public DateTime? DueDateFrom { get; set; }
        public DateTime? DueDateTo { get; set; }
    }
}
