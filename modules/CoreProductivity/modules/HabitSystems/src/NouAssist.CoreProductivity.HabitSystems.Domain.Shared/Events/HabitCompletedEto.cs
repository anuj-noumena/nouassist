using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace NouAssist.CoreProductivity.HabitSystems.Events
{
    [EventName("NouAssist.CoreProductivity.HabitSystems.Completed")]
    public class HabitCompletedEto : IMultiTenant
    {
        public Guid HabitId { get; set; }
        public Guid? TenantId { get; set; }
        public DateTime OccurredAt { get; set; }
        public DateTime LogDate { get; set; }
        public int CurrentStreak { get; set; }
    }
}
