using System;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.FinancialPlanning.Events
{
    [EventName("FinancialManagement.FinancialPlanning.ExpenseCreated")]
    public class ExpenseCreatedEto : IMultiTenant
    {
        public string IdempotencyKey { get; set; }
        public Guid ExpenseId { get; set; }
        public Guid UserId { get; set; }
        public MonetaryValue Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public Guid CategoryId { get; set; }
        public Guid PayeeId { get; set; }
        public Guid? ProjectId { get; set; }
        public bool IsReimbursable { get; set; }
        public Guid? TenantId { get; set; }
    }
}
