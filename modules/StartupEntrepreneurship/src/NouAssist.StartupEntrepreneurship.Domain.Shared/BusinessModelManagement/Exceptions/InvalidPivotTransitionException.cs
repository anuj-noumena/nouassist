using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Exceptions
{
    /// <summary>
    /// Exception thrown when an invalid pivot status transition is attempted
    /// </summary>
    public class InvalidPivotTransitionException : BusinessModelManagementDomainException
    {
        public InvalidPivotTransitionException(PivotStatus currentStatus, PivotStatus targetStatus)
            : base("BusinessModelManagement:ErrorMessage:InvalidPivotTransition")
        {
            WithData("currentStatus", currentStatus);
            WithData("targetStatus", targetStatus);
        }
    }
}
