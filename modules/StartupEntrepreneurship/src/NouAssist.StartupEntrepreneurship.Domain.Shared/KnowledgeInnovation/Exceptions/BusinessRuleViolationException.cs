namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Exceptions
{
    /// <summary>
    /// Exception thrown when a business rule is violated
    /// </summary>
    public class BusinessRuleViolationException : KnowledgeInnovationDomainException
    {
        public BusinessRuleViolationException(string message)
            : base("KnowledgeInnovation:ErrorMessage:BusinessRuleViolation", message)
        {
        }
    }
}
