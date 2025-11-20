using System;
using NouAssist.FinancialManagement.FinancialPlanning.Enums;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.Integration.Events
{
    [EventName("NouAssist.Finance.FinancialGoalCreated")]
    public class FinancialGoalCreatedEto : IMultiTenant
    {
        public Guid GoalId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid UserId { get; set; }
        public string GoalName { get; set; }
        public string Description { get; set; }
        public GoalType GoalType { get; set; }
        public MonetaryValue TargetAmount { get; set; }
        public MonetaryValue CurrentAmount { get; set; }
        public DateTime TargetDate { get; set; }
        public DateTime StartDate { get; set; }
        public bool RequiresActionPlan { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
