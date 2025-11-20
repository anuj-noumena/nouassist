using System;
using NouAssist.FinancialManagement.FinancialPlanning.Enums;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.FinancialPlanning.Events
{
    [EventName("NouAssist.FinancialPlanning.ExpenseCreated")]
    public class ExpenseCreatedEto : IMultiTenant
    {
        public string IdempotencyKey { get; set; }
        public Guid ExpenseId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime ExpenseDate { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public bool IsReimbursable { get; set; }
        public DateTime CreatedAt { get; set; }

        // For Budget tracking integration
        public Guid? BudgetId { get; set; }
        public string PaymentMethod { get; set; }
        public bool IsTaxDeductible { get; set; }
        public string Tags { get; set; }
        public ExpenseStatus Status { get; set; }
    }
}
