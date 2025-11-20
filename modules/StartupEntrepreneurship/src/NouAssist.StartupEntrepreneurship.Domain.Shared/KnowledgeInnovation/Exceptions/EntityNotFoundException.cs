namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Exceptions
{
    /// <summary>
    /// Exception thrown when an entity is not found
    /// </summary>
    public class EntityNotFoundException : KnowledgeInnovationDomainException
    {
        public EntityNotFoundException(string entityType, object id)
            : base("KnowledgeInnovation:ErrorMessage:EntityNotFound")
        {
            WithData("entityType", entityType);
            WithData("id", id);
        }
    }
}
