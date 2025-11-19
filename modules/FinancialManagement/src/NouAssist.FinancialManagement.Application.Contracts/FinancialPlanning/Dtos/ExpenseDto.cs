using System;
using NouAssist.FinancialManagement.FinancialPlanning.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.FinancialManagement.FinancialPlanning.Dtos
{
    public class ExpenseDto : FullAuditedEntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime ExpenseDate { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public Guid PayeeId { get; set; }
        public string PayeeName { get; set; }
        public string Description { get; set; }
        public ExpenseStatus Status { get; set; }
        public Guid? ReceiptDocumentId { get; set; }
        public bool IsReimbursable { get; set; }
        public bool IsBillable { get; set; }
    }

    public class CreateExpenseDto
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";
        public DateTime ExpenseDate { get; set; }
        public Guid CategoryId { get; set; }
        public Guid PayeeId { get; set; }
        public string Description { get; set; }
        public bool IsReimbursable { get; set; }
        public bool IsBillable { get; set; }
        public Guid? ReceiptDocumentId { get; set; }
        public Guid? ProjectId { get; set; }
    }

    public class SubmitExpenseDto
    {
        public Guid ExpenseId { get; set; }
    }

    public class ApproveExpenseDto
    {
        public Guid ExpenseId { get; set; }
        public bool IsApproved { get; set; }
        public string Comments { get; set; }
    }

    public class GetExpenseListDto : PagedAndSortedResultRequestDto
    {
        public Guid? UserFilter { get; set; }
        public Guid? CategoryFilter { get; set; }
        public ExpenseStatus? StatusFilter { get; set; }
        public DateTime? ExpenseDateFrom { get; set; }
        public DateTime? ExpenseDateTo { get; set; }
    }
}
