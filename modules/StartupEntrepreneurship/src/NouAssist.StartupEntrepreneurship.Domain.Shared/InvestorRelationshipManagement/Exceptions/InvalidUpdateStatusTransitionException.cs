namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Exceptions
{
    /// <summary>
    /// Exception thrown when invalid update status transition is attempted
    /// </summary>
    public class InvalidUpdateStatusTransitionException : IRMDomainException
    {
        public InvalidUpdateStatusTransitionException(UpdateStatus fromStatus, UpdateStatus toStatus)
            : base("IRM:ErrorMessage:InvalidUpdateStatusTransition")
        {
            WithData("fromStatus", fromStatus);
            WithData("toStatus", toStatus);
        }
    }
}
