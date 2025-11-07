using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.CoreProductivity.HabitSystems.Tracking.Dtos
{
    public class HealthTrackingDto : AuditedEntityDto<Guid>
    {
        public DateTime Date { get; set; }
        public int SleepHours { get; set; }
        public int WaterCount { get; set; }
        public int PostureChecks { get; set; }
        public int BreaksTaken { get; set; }
        public string Notes { get; set; }
        
        public bool HasGoodSleep { get; set; }
        public bool HasAdequateWater { get; set; }
        public bool TookBreaks { get; set; }
    }

    public class UpdateHealthTrackingDto
    {
        public DateTime Date { get; set; }
        public int? SleepHours { get; set; }
        public int? WaterCount { get; set; }
        public int? PostureChecks { get; set; }
        public int? BreaksTaken { get; set; }
        public string Notes { get; set; }
    }

    public class LogHealthActionDto
    {
        public DateTime Date { get; set; }
        public HealthAction Action { get; set; }
        public int? Value { get; set; }
    }

    public enum HealthAction
    {
        LogSleep,
        LogWater,
        LogPostureCheck,
        LogBreak
    }
}
