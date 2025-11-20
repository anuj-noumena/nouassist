namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Exceptions
{
    /// <summary>
    /// Exception thrown when attempting to transition a lead to an invalid status
    /// </summary>
    public class InvalidLeadStatusTransitionException : CRMDomainException
    {
        public InvalidLeadStatusTransitionException(LeadStatus currentStatus, LeadStatus newStatus)
            : base("CRM:ErrorMessage:InvalidLeadStatusTransition")
        {
            WithData("currentStatus", currentStatus);
            WithData("newStatus", newStatus);
        }
    }
}
