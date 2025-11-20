namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Exceptions
{
    /// <summary>
    /// Exception thrown when an invalid status transition is attempted
    /// </summary>
    public class InvalidStatusTransitionException : KnowledgeInnovationDomainException
    {
        public InvalidStatusTransitionException(string entityType, object currentStatus, object targetStatus)
            : base("KnowledgeInnovation:ErrorMessage:InvalidStatusTransition")
        {
            WithData("entityType", entityType);
            WithData("currentStatus", currentStatus);
            WithData("targetStatus", targetStatus);
        }
    }
}
