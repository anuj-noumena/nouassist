using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.PersonalDevelopment.Habits.Entities
{
    public class HabitReward : Entity<Guid>
    {
        public Guid HabitId { get; private set; }
        public string Description { get; private set; }
        public string Craving { get; set; }
        public string RewardType { get; set; }

        protected HabitReward() { /* ORM */ }

        public HabitReward(Guid id, Guid habitId, string description, string craving = null)
        {
            Id = id;
            HabitId = habitId;
            Description = Check.NotNullOrWhiteSpace(description, nameof(description), maxLength: 500);
            Craving = craving;
        }

        public bool HasCraving => !string.IsNullOrWhiteSpace(Craving);

        public void UpdateCraving(string craving)
        {
            Craving = craving;
        }
    }
}
