using System;
using NouAssist.FinancialManagement.FinancialPlanning.Enums;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.Application.Dtos;

namespace NouAssist.FinancialManagement.FinancialPlanning.Dtos
{
    public class ExpenseDto : FullAuditedEntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public MonetaryValue Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public ExpenseStatus Status { get; set; }
        public bool IsReimbursable { get; set; }
        public Guid? ProjectId { get; set; }
    }

    public class CreateExpenseDto
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime ExpenseDate { get; set; }
        public Guid CategoryId { get; set; }
        public Guid PayeeId { get; set; }
        public string Description { get; set; }
        public bool IsReimbursable { get; set; }
        public bool IsBillable { get; set; }
        public Guid? ProjectId { get; set; }
        public Guid? ReceiptDocumentId { get; set; }
    }

    public class GetExpenseListDto : PagedAndSortedResultRequestDto
    {
        public Guid? UserId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid? CategoryId { get; set; }
        public ExpenseStatus? Status { get; set; }
    }

    public class ApproveExpenseDto
    {
        public bool IsApproved { get; set; }
        public string Comments { get; set; }
    }
}
