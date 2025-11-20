using System;
using Volo.Abp.Domain.Values;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.ValueObjects
{
    /// <summary>
    /// Value object representing a priority score for recommendations and tasks
    /// </summary>
    public class PriorityScore : ValueObject
    {
        /// <summary>
        /// Value of the priority score (0-100)
        /// </summary>
        public int Value { get; private set; }

        protected PriorityScore()
        {
        }

        /// <summary>
        /// Creates a new priority score
        /// </summary>
        /// <param name="value">Priority value between 0 and 100</param>
        public PriorityScore(int value)
        {
            Value = Math.Clamp(value, 0, 100);
        }

        /// <summary>
        /// Gets the priority level as a readable category
        /// </summary>
        /// <returns>Priority level category</returns>
        public string GetPriorityLevel()
        {
            if (Value < 20) return "Very Low";
            if (Value < 40) return "Low";
            if (Value < 60) return "Medium";
            if (Value < 80) return "High";
            return "Very High";
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
