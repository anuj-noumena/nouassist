using System;
using NouAssist.PersonalDevelopment.Habits.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.PersonalDevelopment.Habits.Entities
{
    public class HabitCue : Entity<Guid>
    {
        public Guid HabitId { get; private set; }
        public CueType CueType { get; private set; }
        public string Description { get; private set; }
        public string CueValue { get; set; }
        public DateTime CreatedAt { get; private set; }

        protected HabitCue() { /* ORM */ }

        public HabitCue(Guid id, Guid habitId, CueType cueType, string description, string cueValue = null)
        {
            Id = id;
            HabitId = habitId;
            CueType = cueType;
            Description = Check.NotNullOrWhiteSpace(description, nameof(description), maxLength: 500);
            CueValue = cueValue;
            CreatedAt = DateTime.UtcNow;
        }

        public string GetCueTypeDescription()
        {
            return CueType switch
            {
                CueType.Time => "Time-based",
                CueType.Location => "Location-based",
                CueType.PrecedingEvent => "After another action",
                CueType.EmotionalState => "Emotional state",
                CueType.OtherPeople => "With certain people",
                CueType.Other => "Other",
                _ => "Unknown"
            };
        }
    }
}
