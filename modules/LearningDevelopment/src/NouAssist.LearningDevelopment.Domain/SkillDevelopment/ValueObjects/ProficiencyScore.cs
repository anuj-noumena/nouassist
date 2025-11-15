using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.SkillDevelopment.ValueObjects
{
    public class ProficiencyScore : ValueObject
    {
        public int Score { get; private set; }
        public DateTime AssessedAt { get; private set; }
        public string AssessmentNotes { get; private set; }

        private ProficiencyScore() { /* ORM */ }

        public ProficiencyScore(int score, DateTime assessedAt, string notes = null)
        {
            if (score < 0 || score > 100)
            {
                throw new BusinessException("SkillDevelopment:InvalidProficiencyScore")
                    .WithData("Score", score);
            }

            Score = score;
            AssessedAt = assessedAt;
            AssessmentNotes = notes ?? string.Empty;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Score;
            yield return AssessedAt;
            yield return AssessmentNotes;
        }

        public bool IsAtLevel(int threshold) => Score >= threshold;
    }
}
