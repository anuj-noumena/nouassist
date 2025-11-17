using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Domain.Values;

namespace NouAssist.LearningDevelopment.SkillDevelopment.ValueObjects
{
    public class AssessmentResult : ValueObject
    {
        public Dictionary<string, int> Scores { get; private set; }
        public List<string> StrengthAreas { get; private set; }
        public List<string> ImprovementAreas { get; private set; }
        public string OverallNotes { get; private set; }

        private AssessmentResult() { /* ORM */ }

        public AssessmentResult(
            Dictionary<string, int> scores,
            List<string> strengthAreas,
            List<string> improvementAreas,
            string overallNotes = null)
        {
            Scores = scores ?? new Dictionary<string, int>();
            StrengthAreas = strengthAreas ?? new List<string>();
            ImprovementAreas = improvementAreas ?? new List<string>();
            OverallNotes = overallNotes ?? string.Empty;
        }

        public int GetAverageScore() => Scores.Any() ? (int)Scores.Values.Average() : 0;

        protected override IEnumerable<object> GetAtomicValues()
        {
            foreach (var score in Scores.OrderBy(x => x.Key))
            {
                yield return score.Key;
                yield return score.Value;
            }
            foreach (var area in StrengthAreas.OrderBy(x => x))
                yield return area;
            foreach (var area in ImprovementAreas.OrderBy(x => x))
                yield return area;
            yield return OverallNotes;
        }
    }
}
