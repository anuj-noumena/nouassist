namespace NouAssist.BusinessOperations.FreelanceOperations.ValueObjects
{
    /// <summary>
    /// Value object representing details of a project milestone
    /// </summary>
    public class MilestoneDetails : ValueObject
    {
        /// <summary>
        /// Title of the milestone
        /// </summary>
        public string Title { get; private set; }
        
        /// <summary>
        /// Description of the milestone
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// Price for completing the milestone
        /// </summary>
        public decimal Price { get; private set; }
        
        /// <summary>
        /// Currency code (e.g., USD, EUR)
        /// </summary>
        public string Currency { get; private set; }
        
        /// <summary>
        /// Acceptance criteria for the milestone
        /// </summary>
        public string AcceptanceCriteria { get; private set; }

        protected MilestoneDetails()
        {
        }

        public MilestoneDetails(
            string title, 
            string description, 
            decimal price, 
            string currency,
            string acceptanceCriteria = null)
        {
            Title = Check.NotNullOrWhiteSpace(title, nameof(title));
            Description = Check.NotNullOrWhiteSpace(description, nameof(description));
            
            if (price < 0)
            {
                throw new BusinessException("FreelanceOperations:Validation:InvalidValue", "Milestone price");
            }
            
            Price = price;
            Currency = Check.NotNullOrWhiteSpace(currency, nameof(currency)).ToUpperInvariant();
            AcceptanceCriteria = acceptanceCriteria;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Title;
            yield return Description;
            yield return Price;
            yield return Currency;
            yield return AcceptanceCriteria ?? string.Empty;
        }
    }
}
