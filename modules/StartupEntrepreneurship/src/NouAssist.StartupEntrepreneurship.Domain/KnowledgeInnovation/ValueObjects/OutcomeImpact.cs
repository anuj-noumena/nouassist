namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.ValueObjects
{
    /// <summary>
    /// Value object representing the impact of a decision outcome
    /// </summary>
    public class OutcomeImpact : ValueObject
    {
        public string Area { get; private set; } // Business area impacted
        public string Description { get; private set; }
        public int Magnitude { get; private set; } // 1-10 scale for impact size

        private OutcomeImpact() { }

        public OutcomeImpact(string area, string description, int magnitude)
        {
            Area = Check.NotNullOrWhiteSpace(
                area, 
                nameof(area), 
                maxLength: KnowledgeInnovationConsts.DefaultLengths.Name);
            
            Description = Check.NotNullOrWhiteSpace(
                description, 
                nameof(description), 
                maxLength: KnowledgeInnovationConsts.DefaultLengths.ShortDescription);
            
            Magnitude = Check.Range(magnitude, nameof(magnitude), 1, 10);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Area;
            yield return Description;
            yield return Magnitude;
        }
    }
}
