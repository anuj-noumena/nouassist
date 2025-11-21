using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.ValueObjects
{
    /// <summary>
    /// Value object representing criteria for evaluating ideas
    /// </summary>
    public class EvaluationCriteria : ValueObject
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int Weight { get; private set; } // 1-10 scale for importance

        private EvaluationCriteria() { }

        public EvaluationCriteria(string name, string description, int weight)
        {
            Name = Check.NotNullOrWhiteSpace(
                name, 
                nameof(name), 
                maxLength: KnowledgeInnovationConsts.DefaultLengths.Name);
            
            Description = description;
            Weight = Check.Range(weight, nameof(weight), 1, 10);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
            yield return Description;
            yield return Weight;
        }
    }
}
