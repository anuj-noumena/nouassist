using System.Collections.Generic;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.ValueObjects
{
    /// <summary>
    /// Value object representing a goal description with validation
    /// </summary>
    public class GoalDescription : ValueObject
    {
        public string Title { get; private set; }
        public string Description { get; private set; }

        private GoalDescription() { }

        public GoalDescription(string title, string description)
        {
            SetTitle(title);
            SetDescription(description);
        }

        private void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(
                title,
                nameof(title),
                maxLength: StrategicPlanningConsts.MaxGoalTitleLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: StrategicPlanningConsts.MaxGoalDescriptionLength
            );
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Title;
            yield return Description;
        }
    }
}
