using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.FinancialPlanning.Events
{
    [EventName("NouAssist.FinancialPlanning.FinancialGoalCreated")]
    public class FinancialGoalCreatedEto : IMultiTenant
    {
        public Guid GoalId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string GoalType { get; set; }
        public decimal TargetAmount { get; set; }
        public DateTime TargetDate { get; set; }
        public string Currency { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
