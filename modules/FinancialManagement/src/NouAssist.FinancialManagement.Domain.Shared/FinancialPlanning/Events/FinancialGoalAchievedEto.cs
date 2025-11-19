using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.FinancialPlanning.Events
{
    [EventName("NouAssist.FinancialPlanning.FinancialGoalAchieved")]
    public class FinancialGoalAchievedEto : IMultiTenant
    {
        public Guid GoalId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public decimal TargetAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public string Currency { get; set; }
        public DateTime TargetDate { get; set; }
        public DateTime AchievedAt { get; set; }
        public int DaysAheadOrBehind { get; set; }
    }
}
