using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.CoreProductivity.HabitSystems.Tracking.Dtos
{
    public class FocusSessionDto : AuditedEntityDto<Guid>
    {
        public DateTime Date { get; set; }
        public int CompletedPomodoros { get; set; }
        public int TotalFocusTime { get; set; }
        public Guid? TaskId { get; set; }
        public string Notes { get; set; }
        
        public bool HasTaskLinked { get; set; }
        public double AveragePomodoroLength { get; set; }
        public double TotalFocusHours { get; set; }
    }

    public class CompletePomodoroDto
    {
        public DateTime Date { get; set; }
        public int DurationMinutes { get; set; } = 25;
    }

    public class UpdateFocusSessionDto
    {
        public DateTime Date { get; set; }
        public Guid? TaskId { get; set; }
        public string Notes { get; set; }
    }
}
