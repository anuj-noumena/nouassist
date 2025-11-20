namespace NouAssist.RequirementsManagement.Elicitation.Exceptions
{
    /// <summary>
    /// Exception thrown when an invalid session status transition is attempted
    /// </summary>
    public class InvalidSessionStatusTransitionException : ElicitationDomainException
    {
        public InvalidSessionStatusTransitionException(SessionStatus currentStatus, SessionStatus targetStatus)
            : base("Elicitation:ErrorMessage:InvalidSessionStatusTransition")
        {
            WithData("currentStatus", currentStatus);
            WithData("targetStatus", targetStatus);
        }
    }
}
