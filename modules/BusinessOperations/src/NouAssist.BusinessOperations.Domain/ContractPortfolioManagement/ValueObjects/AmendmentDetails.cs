namespace NouAssist.BusinessOperations.ContractPortfolioManagement.ValueObjects
{
    /// <summary>
    /// Value object representing details of a contract amendment
    /// </summary>
    public class AmendmentDetails : ValueObject
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime EffectiveDate { get; private set; }
        public List<string> AffectedSections { get; private set; }

        protected AmendmentDetails() 
        {
            AffectedSections = new List<string>();
        }

        public AmendmentDetails(string title, string description, DateTime effectiveDate, List<string> affectedSections)
        {
            Title = Check.NotNullOrWhiteSpace(title, nameof(title));
            Description = Check.NotNullOrWhiteSpace(description, nameof(description));
            EffectiveDate = effectiveDate;
            AffectedSections = affectedSections ?? new List<string>();
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Title;
            yield return Description;
            yield return EffectiveDate;
            
            foreach (var section in AffectedSections.OrderBy(s => s))
            {
                yield return section;
            }
        }
    }
}
