namespace NouAssist.BusinessOperations.ContractPortfolioManagement.ValueObjects
{
    /// <summary>
    /// Value object representing a document identifier in an external document management system
    /// </summary>
    public class DocumentId : ValueObject
    {
        public Guid Id { get; private set; }
        public string RepositoryType { get; private set; }

        protected DocumentId() { }

        public DocumentId(Guid id, string repositoryType)
        {
            Id = id;
            RepositoryType = Check.NotNullOrWhiteSpace(repositoryType, nameof(repositoryType));
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
            yield return RepositoryType;
        }
    }
}
