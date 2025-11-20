namespace NouAssist.RequirementsManagement.ChangeManagement.Exceptions
{
    /// <summary>
    /// Exception thrown when an invalid status transition is attempted
    /// </summary>
    public class InvalidStatusTransitionException : ChangeManagementDomainException
    {
        public InvalidStatusTransitionException(RequestStatus currentStatus, RequestStatus targetStatus)
            : base("ChangeManagement:ErrorMessage:InvalidStatusTransition")
        {
            WithData("currentStatus", currentStatus);
            WithData("targetStatus", targetStatus);
        }
    }
}
