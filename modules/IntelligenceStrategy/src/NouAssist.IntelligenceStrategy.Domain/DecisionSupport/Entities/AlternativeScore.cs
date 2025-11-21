using System;
using Volo.Abp.Domain.Entities;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.Entities
{
    /// <summary>
    /// Represents a score for an alternative on a specific criterion
    /// </summary>
    public class AlternativeScore : Entity
    {
        /// <summary>
        /// The alternative being scored
        /// </summary>
        public Guid AlternativeId { get; private set; }
        
        /// <summary>
        /// The criterion being scored against
        /// </summary>
        public Guid CriterionId { get; private set; }
        
        /// <summary>
        /// Score value (normalized between 0 and 1)
        /// </summary>
        public decimal Score { get; private set; }

        protected AlternativeScore() { }

        /// <summary>
        /// Creates a new instance of <see cref="AlternativeScore"/>
        /// </summary>
        internal AlternativeScore(
            Guid alternativeId,
            Guid criterionId,
            decimal score)
        {
            AlternativeId = alternativeId;
            CriterionId = criterionId;
            SetScore(score);
        }

        private void SetScore(decimal score)
        {
            if (score < 0 || score > 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(score),
                    "Score must be between 0 and 1");
            }
            
            Score = score;
        }
        
        /// <summary>
        /// Updates the score value
        /// </summary>
        internal void UpdateScore(decimal score)
        {
            SetScore(score);
        }

        /// <inheritdoc/>
        public override object[] GetKeys()
        {
            return new object[] { AlternativeId, CriterionId };
        }
    }
}
