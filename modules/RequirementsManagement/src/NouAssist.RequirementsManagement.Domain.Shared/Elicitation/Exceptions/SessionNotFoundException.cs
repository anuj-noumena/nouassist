namespace NouAssist.RequirementsManagement.Elicitation.Exceptions
{
    /// <summary>
    /// Exception thrown when an elicitation session is not found
    /// </summary>
    public class SessionNotFoundException : ElicitationDomainException
    {
        public SessionNotFoundException(Guid sessionId)
            : base("Elicitation:ErrorMessage:SessionNotFound")
        {
            WithData("sessionId", sessionId);
        }
    }
}
