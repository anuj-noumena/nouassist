using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.ValueObjects
{
    /// <summary>
    /// Represents an item in a SWOT analysis (Strength, Weakness, Opportunity, Threat)
    /// </summary>
    public class SwotItem : ValueObject
    {
        public enum SwotCategory
        {
            Strength = 1,
            Weakness = 2,
            Opportunity = 3,
            Threat = 4
        }

        /// <summary>
        /// The category of the SWOT item
        /// </summary>
        public SwotCategory Category { get; private set; }
        
        /// <summary>
        /// The description of the SWOT item
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// The impact level of this item (1-5)
        /// </summary>
        public int Impact { get; private set; }

        private SwotItem()
        {
        }

        /// <summary>
        /// Creates a new instance of SwotItem
        /// </summary>
        /// <param name="category">The category of the SWOT item</param>
        /// <param name="description">The description of the SWOT item</param>
        /// <param name="impact">The impact level (1-5)</param>
        public SwotItem(SwotCategory category, string description, int impact)
        {
            Category = category;
            Description = Check.NotNullOrWhiteSpace(description, nameof(description));
            Impact = Check.Range(impact, nameof(impact), 1, 5);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Category;
            yield return Description;
            yield return Impact;
        }
    }
}
