using System;
using NouAssist.FinancialManagement.Shared.ValueObjects;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.FinancialManagement.Integration.Events
{
    [EventName("NouAssist.Finance.GoalCheckInDue")]
    public class GoalCheckInDueEto : IMultiTenant
    {
        public Guid GoalId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid UserId { get; set; }
        public string GoalName { get; set; }
        public MonetaryValue TargetAmount { get; set; }
        public MonetaryValue CurrentAmount { get; set; }
        public decimal ProgressPercent { get; set; }
        public DateTime TargetDate { get; set; }
        public DateTime LastCheckInDate { get; set; }
        public string CheckInFrequency { get; set; } // Weekly, Monthly, Quarterly
        public DateTime CheckInDueDate { get; set; }
    }
}
