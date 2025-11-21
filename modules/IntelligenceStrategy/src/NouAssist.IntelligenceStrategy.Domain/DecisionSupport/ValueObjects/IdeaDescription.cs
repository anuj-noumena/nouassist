using System.Collections.Generic;
using NouAssist.IntelligenceStrategy.DecisionSupport.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.ValueObjects
{
    /// <summary>
    /// Represents a description of an innovation idea
    /// </summary>
    public class IdeaDescription : ValueObject
    {
        /// <summary>
        /// The title of the idea
        /// </summary>
        public string Title { get; private set; }
        
        /// <summary>
        /// The detailed description of the idea
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// The expected benefits of implementing the idea
        /// </summary>
        public string ExpectedBenefits { get; private set; }
        
        /// <summary>
        /// The potential challenges in implementing the idea
        /// </summary>
        public string PotentialChallenges { get; private set; }

        private IdeaDescription()
        {
        }

        /// <summary>
        /// Creates a new instance of IdeaDescription
        /// </summary>
        /// <param name="title">The title of the idea</param>
        /// <param name="description">The detailed description</param>
        /// <param name="expectedBenefits">The expected benefits</param>
        /// <param name="potentialChallenges">The potential challenges</param>
        public IdeaDescription(
            string title,
            string description,
            string expectedBenefits,
            string potentialChallenges)
        {
            Title = Check.NotNullOrWhiteSpace(
                title,
                nameof(title),
                maxLength: DecisionSupportConsts.MaxInnovationIdeaTitleLength);
                
            Description = Check.NotNullOrWhiteSpace(description, nameof(description));
            ExpectedBenefits = expectedBenefits;
            PotentialChallenges = potentialChallenges;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Title;
            yield return Description;
            yield return ExpectedBenefits;
            yield return PotentialChallenges;
        }
    }
}
