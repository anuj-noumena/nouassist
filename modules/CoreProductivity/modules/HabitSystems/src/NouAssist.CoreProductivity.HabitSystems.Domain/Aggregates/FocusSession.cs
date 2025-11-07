using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.CoreProductivity.HabitSystems.Aggregates
{
    public class FocusSession : AuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid OwnerId { get; private set; }
        public DateTime Date { get; private set; }
        public int CompletedPomodoros { get; private set; }
        public int TotalFocusTime { get; private set; }
        public Guid? TaskId { get; private set; }
        public string Notes { get; set; }
        public Guid? TenantId { get; private set; }

        protected FocusSession() { /* ORM */ }

        public FocusSession(
            Guid id,
            Guid ownerId,
            DateTime date,
            Guid? taskId = null,
            Guid? tenantId = null
        ) : base(id)
        {
            OwnerId = Check.NotNull(ownerId, nameof(ownerId));
            Date = new DateTime(date.Year, date.Month, date.Day);
            TaskId = taskId;
            TenantId = tenantId;
            CompletedPomodoros = 0;
            TotalFocusTime = 0;
        }

        public bool HasTaskLinked => TaskId.HasValue;
        public double AveragePomodoroLength => CompletedPomodoros == 0 ? 0 : (double)TotalFocusTime / CompletedPomodoros;
        public double TotalFocusHours => TotalFocusTime / 60.0;

        public void CompletePomodoro(int durationMinutes = 25)
        {
            if (durationMinutes <= 0 || durationMinutes > 120)
            {
                throw new BusinessException("Habits:InvalidPomodoroDuration", 
                    "Pomodoro duration must be between 1 and 120 minutes");
            }

            if (CompletedPomodoros >= 100)
            {
                throw new BusinessException("Habits:MaxPomodorosReached", 
                    "Maximum pomodoros per day reached");
            }

            CompletedPomodoros++;
            TotalFocusTime += durationMinutes;
        }

        public void AddFocusTime(int minutes)
        {
            if (minutes <= 0)
            {
                throw new BusinessException("Habits:InvalidFocusTime", 
                    "Focus time must be positive");
            }

            TotalFocusTime += minutes;
        }

        public void LinkTask(Guid taskId)
        {
            TaskId = taskId;
        }

        public void UnlinkTask()
        {
            TaskId = null;
        }

        public FocusMetrics GetMetrics()
        {
            return new FocusMetrics
            {
                CompletedPomodoros = CompletedPomodoros,
                TotalFocusTime = TotalFocusTime,
                TotalFocusHours = TotalFocusHours,
                AveragePomodoroLength = AveragePomodoroLength
            };
        }
    }

    public class FocusMetrics
    {
        public int CompletedPomodoros { get; set; }
        public int TotalFocusTime { get; set; }
        public double TotalFocusHours { get; set; }
        public double AveragePomodoroLength { get; set; }
    }
}
