using System;
using System.Collections.Generic;
using NouAssist.FinancialManagement.AccountsPayable.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.FinancialManagement.AccountsPayable.Dtos
{
    public class BillDto : FullAuditedEntityDto<Guid>
    {
        public string BillNumber { get; set; }
        public Guid VendorId { get; set; }
        public string VendorName { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public BillStatus Status { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public Guid? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public bool RequiresApproval { get; set; }
        public int DaysUntilDue { get; set; }
        public bool IsOverdue { get; set; }
        public List<BillApprovalDto> Approvals { get; set; }
        public List<BillLineItemDto> LineItems { get; set; }
    }

    public class BillApprovalDto
    {
        public Guid ApproverId { get; set; }
        public string ApproverName { get; set; }
        public ApprovalLevel Level { get; set; }
        public DateTime ApprovedDate { get; set; }
        public string Comments { get; set; }
    }

    public class BillLineItemDto
    {
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
        public string Currency { get; set; }
    }

    public class CreateBillDto
    {
        public string BillNumber { get; set; }
        public Guid VendorId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public Guid CategoryId { get; set; }
        public Guid? ProjectId { get; set; }
        public Guid? DocumentId { get; set; }
        public List<CreateBillLineItemDto> LineItems { get; set; }
    }

    public class CreateBillLineItemDto
    {
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public Guid? ProductId { get; set; }
    }

    public class ApproveBillDto
    {
        public Guid BillId { get; set; }
        public ApprovalLevel Level { get; set; }
        public string Comments { get; set; }
    }

    public class PayBillDto
    {
        public Guid BillId { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public string ConfirmationNumber { get; set; }
    }

    public class GetBillListDto : PagedAndSortedResultRequestDto
    {
        public Guid? VendorFilter { get; set; }
        public BillStatus? StatusFilter { get; set; }
        public DateTime? DueDateFrom { get; set; }
        public DateTime? DueDateTo { get; set; }
        public bool? OverdueOnly { get; set; }
    }
}
