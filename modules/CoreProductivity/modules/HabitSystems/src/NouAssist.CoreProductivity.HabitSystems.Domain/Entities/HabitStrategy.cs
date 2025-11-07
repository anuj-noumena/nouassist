using System;
using NouAssist.CoreProductivity.HabitSystems.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.CoreProductivity.HabitSystems.Entities
{
    public class HabitStrategy : Entity<Guid>
    {
        public Guid HabitId { get; private set; }
        public HabitStrategyType StrategyType { get; private set; }
        public string Description { get; private set; }
        public DateTime CreatedAt { get; private set; }

        protected HabitStrategy() { /* ORM */ }

        public HabitStrategy(Guid id, Guid habitId, HabitStrategyType strategyType, string description)
        {
            Id = id;
            HabitId = habitId;
            StrategyType = strategyType;
            Description = Check.NotNullOrWhiteSpace(description, nameof(description), maxLength: 1000);
            CreatedAt = DateTime.UtcNow;
        }

        public string GetStrategyTypeDescription()
        {
            return StrategyType switch
            {
                HabitStrategyType.MakeCueObvious => "Make it Obvious",
                HabitStrategyType.MakeResponseAttractive => "Make it Attractive",
                HabitStrategyType.MakeResponseEasy => "Make it Easy",
                HabitStrategyType.MakeRewardSatisfying => "Make it Satisfying",
                HabitStrategyType.MakeCueInvisible => "Make it Invisible (breaking habit)",
                HabitStrategyType.MakeResponseUnattractive => "Make it Unattractive (breaking habit)",
                HabitStrategyType.MakeResponseDifficult => "Make it Difficult (breaking habit)",
                HabitStrategyType.MakeRewardUnsatisfying => "Make it Unsatisfying (breaking habit)",
                _ => "Unknown"
            };
        }
    }
}
