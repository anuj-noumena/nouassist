using System;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.Integration.Events
{
    [EventName("NouAssist.Finance.GoalMilestoneReached")]
    public class GoalMilestoneReachedEto : IMultiTenant
    {
        public Guid GoalId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid UserId { get; set; }
        public string GoalName { get; set; }
        public int MilestonePercent { get; set; } // 25, 50, 75
        public MonetaryValue TargetAmount { get; set; }
        public MonetaryValue CurrentAmount { get; set; }
        public MonetaryValue RemainingAmount { get; set; }
        public DateTime TargetDate { get; set; }
        public int DaysRemaining { get; set; }
        public DateTime AchievedAt { get; set; }
    }
}
