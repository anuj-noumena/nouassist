using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.ValueObjects
{
    /// <summary>
    /// Value object representing a score for idea evaluation
    /// </summary>
    public class EvaluationScore : ValueObject
    {
        public string CriteriaName { get; private set; }
        public int Score { get; private set; } // 1-10 scale
        public string Justification { get; private set; }

        private EvaluationScore() { }

        public EvaluationScore(string criteriaName, int score, string justification)
        {
            CriteriaName = Check.NotNullOrWhiteSpace(
                criteriaName, 
                nameof(criteriaName), 
                maxLength: KnowledgeInnovationConsts.DefaultLengths.Name);
            
            Score = Check.Range(score, nameof(score), 1, 10);
            Justification = justification;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return CriteriaName;
            yield return Score;
            yield return Justification;
        }
    }
}
