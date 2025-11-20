namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.ValueObjects
{
    /// <summary>
    /// Value object representing the source of an idea
    /// </summary>
    public class IdeaSource : ValueObject
    {
        public string SourceName { get; private set; }
        public string SourceDescription { get; private set; }
        public DateTime SourcedDate { get; private set; }

        private IdeaSource() { }

        public IdeaSource(string sourceName, string sourceDescription, DateTime sourcedDate)
        {
            SourceName = Check.NotNullOrWhiteSpace(
                sourceName, 
                nameof(sourceName), 
                maxLength: KnowledgeInnovationConsts.DefaultLengths.Name);
            
            SourceDescription = sourceDescription;
            SourcedDate = sourcedDate;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return SourceName;
            yield return SourceDescription;
            yield return SourcedDate;
        }
    }
}
