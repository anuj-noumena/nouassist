using System;
using NouAssist.IntelligenceStrategy.DecisionSupport.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.Entities
{
    /// <summary>
    /// Represents a criterion used in a decision model
    /// </summary>
    public class Criterion : Entity<Guid>
    {
        /// <summary>
        /// The decision model this criterion belongs to
        /// </summary>
        public Guid DecisionModelId { get; private set; }
        
        /// <summary>
        /// Name of the criterion
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// Description of the criterion
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// Weight of this criterion in the decision (normalized between 0 and 1)
        /// </summary>
        public decimal Weight { get; private set; }

        protected Criterion() { }

        /// <summary>
        /// Creates a new instance of <see cref="Criterion"/>
        /// </summary>
        internal Criterion(
            Guid id,
            Guid decisionModelId,
            string name,
            string description,
            decimal weight) : base(id)
        {
            DecisionModelId = decisionModelId;
            SetName(name);
            SetDescription(description);
            SetWeight(weight);
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: 100
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: DecisionSupportConsts.MaxDescriptionLength
            );
        }

        private void SetWeight(decimal weight)
        {
            if (weight < 0 || weight > 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(weight),
                    "Weight must be between 0 and 1");
            }
            
            Weight = weight;
        }
    }
}
