namespace NouAssist.BusinessOperations.FreelanceOperations.Exceptions
{
    /// <summary>
    /// Exception thrown when an invalid status transition is attempted
    /// </summary>
    public class InvalidStatusTransitionException : FreelanceOperationsDomainException
    {
        public InvalidStatusTransitionException(string entityType, string currentStatus, string targetStatus)
            : base("FreelanceOperations:ErrorMessage:InvalidStatusTransition")
        {
            WithData("entityType", entityType);
            WithData("currentStatus", currentStatus);
            WithData("targetStatus", targetStatus);
        }
    }
}
