using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.DecisionSupport.ValueObjects
{
    /// <summary>
    /// Represents an evaluation score for an idea or option
    /// </summary>
    public class EvaluationScore : ValueObject
    {
        /// <summary>
        /// The score value (0-10)
        /// </summary>
        public decimal Value { get; private set; }
        
        /// <summary>
        /// The criterion the score is for
        /// </summary>
        public string Criterion { get; private set; }
        
        /// <summary>
        /// The weight of this criterion (0-1)
        /// </summary>
        public decimal Weight { get; private set; }
        
        /// <summary>
        /// The weighted score (Value * Weight)
        /// </summary>
        public decimal WeightedScore => Value * Weight;
        
        /// <summary>
        /// Comments explaining the score
        /// </summary>
        public string Comments { get; private set; }

        private EvaluationScore()
        {
        }

        /// <summary>
        /// Creates a new instance of EvaluationScore
        /// </summary>
        /// <param name="criterion">The criterion being evaluated</param>
        /// <param name="value">The score value (0-10)</param>
        /// <param name="weight">The weight of this criterion (0-1)</param>
        /// <param name="comments">Comments explaining the score</param>
        public EvaluationScore(
            string criterion,
            decimal value,
            decimal weight,
            string comments = null)
        {
            Criterion = Check.NotNullOrWhiteSpace(criterion, nameof(criterion));
            Value = Check.Range(value, nameof(value), 0, 10);
            Weight = Check.Range(weight, nameof(weight), 0, 1);
            Comments = comments;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
            yield return Criterion;
            yield return Weight;
            yield return Comments;
        }
    }
}
