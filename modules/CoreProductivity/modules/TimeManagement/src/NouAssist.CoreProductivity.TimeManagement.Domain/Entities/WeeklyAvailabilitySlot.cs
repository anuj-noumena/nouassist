using System;
using Volo.Abp.Domain.Entities;

namespace NouAssist.CoreProductivity.TimeManagement.Entities
{
    public class WeeklyAvailabilitySlot : Entity<Guid>
    {
        public Guid UserAvailabilityId { get; private set; }
        public int DayOfWeek { get; private set; } // 0 = Sunday, 6 = Saturday
        public string StartTime { get; private set; } // HH:mm format
        public string EndTime { get; private set; } // HH:mm format
        public bool IsAvailable { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        protected WeeklyAvailabilitySlot() { }

        public WeeklyAvailabilitySlot(
            Guid id,
            Guid userAvailabilityId,
            int dayOfWeek,
            string startTime,
            string endTime,
            bool isAvailable)
            : base(id)
        {
            UserAvailabilityId = userAvailabilityId;
            DayOfWeek = dayOfWeek;
            StartTime = startTime;
            EndTime = endTime;
            IsAvailable = isAvailable;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Update(string startTime, string endTime, bool isAvailable)
        {
            StartTime = startTime;
            EndTime = endTime;
            IsAvailable = isAvailable;
            UpdatedAt = DateTime.UtcNow;
        }

        public bool HasTimeRange => !string.IsNullOrEmpty(StartTime) && !string.IsNullOrEmpty(EndTime);
    }
}
