using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.LearningDevelopment.SkillDevelopment.ValueObjects
{
    public class PracticePlanItem : ValueObject
    {
        public string Activity { get; private set; }
        public int EstimatedMinutes { get; private set; }
        public List<string> Resources { get; private set; }
        public string Objective { get; private set; }

        private PracticePlanItem() { /* ORM */ }

        public PracticePlanItem(string activity, int estimatedMinutes, string objective, List<string> resources = null)
        {
            Activity = Check.NotNullOrWhiteSpace(activity, nameof(activity));
            EstimatedMinutes = estimatedMinutes > 0 ? estimatedMinutes : 30;
            Objective = Check.NotNullOrWhiteSpace(objective, nameof(objective));
            Resources = resources ?? new List<string>();
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Activity;
            yield return EstimatedMinutes;
            yield return Objective;
            foreach (var resource in Resources)
                yield return resource;
        }
    }
}
