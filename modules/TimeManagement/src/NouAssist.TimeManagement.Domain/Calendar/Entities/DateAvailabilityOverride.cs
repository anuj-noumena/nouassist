using System;
using Volo.Abp.Domain.Entities;

namespace NouAssist.TimeManagement.Calendar.Entities
{
    public class DateAvailabilityOverride : Entity<Guid>
    {
        public Guid UserAvailabilityId { get; private set; }
        public DateTime Date { get; private set; }
        public bool IsAvailable { get; private set; }
        public string Reason { get; private set; }
        public string StartTime { get; private set; } // HH:mm format
        public string EndTime { get; private set; } // HH:mm format
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        protected DateAvailabilityOverride() { }

        public DateAvailabilityOverride(
            Guid id,
            Guid userAvailabilityId,
            DateTime date,
            bool isAvailable,
            string reason = null,
            string startTime = null,
            string endTime = null)
            : base(id)
        {
            UserAvailabilityId = userAvailabilityId;
            Date = new DateTime(date.Year, date.Month, date.Day); // Normalize to date only
            IsAvailable = isAvailable;
            Reason = reason;
            StartTime = startTime;
            EndTime = endTime;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Update(bool isAvailable, string reason = null, string startTime = null, string endTime = null)
        {
            IsAvailable = isAvailable;
            if (reason != null)
            {
                Reason = reason;
            }

            if (startTime != null)
            {
                StartTime = startTime;
            }

            if (endTime != null)
            {
                EndTime = endTime;
            }

            UpdatedAt = DateTime.UtcNow;
        }

        public bool HasTimeRange => !string.IsNullOrEmpty(StartTime) && !string.IsNullOrEmpty(EndTime);
    }
}
