namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Exceptions
{
    /// <summary>
    /// Exception thrown when an entity with the same title already exists
    /// </summary>
    public class DuplicateTitleException : KnowledgeInnovationDomainException
    {
        public DuplicateTitleException(string entityType, string title)
            : base("KnowledgeInnovation:ErrorMessage:DuplicateTitle")
        {
            WithData("entityType", entityType);
            WithData("title", title);
        }
    }
}
