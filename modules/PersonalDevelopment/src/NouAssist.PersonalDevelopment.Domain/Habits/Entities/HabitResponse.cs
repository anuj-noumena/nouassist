using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.PersonalDevelopment.Habits.Entities
{
    public class HabitResponse : Entity<Guid>
    {
        public Guid HabitId { get; private set; }
        public string Description { get; private set; }
        public string Details { get; set; }
        public int? DurationMinutes { get; set; }

        protected HabitResponse() { /* ORM */ }

        public HabitResponse(Guid id, Guid habitId, string description, string details = null, int? durationMinutes = null)
        {
            Id = id;
            HabitId = habitId;
            Description = Check.NotNullOrWhiteSpace(description, nameof(description), maxLength: 500);
            Details = details;
            DurationMinutes = durationMinutes;
        }

        public bool HasDuration => DurationMinutes.HasValue && DurationMinutes.Value > 0;

        public void UpdateDuration(int minutes)
        {
            if (minutes < 0)
            {
                throw new BusinessException("Habits:InvalidDuration", 
                    "Duration cannot be negative");
            }
            DurationMinutes = minutes;
        }
    }
}
