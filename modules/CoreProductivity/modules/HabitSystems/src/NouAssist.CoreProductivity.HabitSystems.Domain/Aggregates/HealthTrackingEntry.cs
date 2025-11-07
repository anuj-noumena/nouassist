using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.CoreProductivity.HabitSystems.Aggregates
{
    public class HealthTrackingEntry : AuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid OwnerId { get; private set; }
        public DateTime Date { get; private set; }
        public int SleepHours { get; private set; }
        public int WaterCount { get; private set; }
        public int PostureChecks { get; private set; }
        public int BreaksTaken { get; private set; }
        public string WaterLogs { get; set; }
        public string Notes { get; set; }
        public Guid? TenantId { get; private set; }

        protected HealthTrackingEntry() { /* ORM */ }

        public HealthTrackingEntry(Guid id, Guid ownerId, DateTime date, Guid? tenantId = null) : base(id)
        {
            OwnerId = Check.NotNull(ownerId, nameof(ownerId));
            Date = new DateTime(date.Year, date.Month, date.Day);
            TenantId = tenantId;
            SleepHours = 0;
            WaterCount = 0;
            PostureChecks = 0;
            BreaksTaken = 0;
        }

        // Computed Properties
        public bool HasGoodSleep => SleepHours >= 7 && SleepHours <= 9;
        public bool HasAdequateWater => WaterCount >= 8;
        public bool TookBreaks => BreaksTaken > 0;

        // Actions
        public void LogSleep(int hours)
        {
            if (hours < 0 || hours > 24)
            {
                throw new BusinessException("Habits:InvalidSleepHours", 
                    "Sleep hours must be between 0 and 24");
            }
            SleepHours = hours;
        }

        public void LogWater()
        {
            if (WaterCount >= 20)
            {
                throw new BusinessException("Habits:MaxWaterReached", 
                    "Maximum water count reached");
            }
            WaterCount++;
        }

        public void LogPostureCheck()
        {
            PostureChecks++;
        }

        public void LogBreak()
        {
            BreaksTaken++;
        }

        public HealthMetrics GetMetrics()
        {
            return new HealthMetrics
            {
                HasGoodSleep = HasGoodSleep,
                HasAdequateWater = HasAdequateWater,
                TookBreaks = TookBreaks,
                SleepHours = SleepHours,
                WaterCount = WaterCount,
                PostureChecks = PostureChecks,
                BreaksTaken = BreaksTaken
            };
        }
    }

    public class HealthMetrics
    {
        public bool HasGoodSleep { get; set; }
        public bool HasAdequateWater { get; set; }
        public bool TookBreaks { get; set; }
        public int SleepHours { get; set; }
        public int WaterCount { get; set; }
        public int PostureChecks { get; set; }
        public int BreaksTaken { get; set; }
    }
}
