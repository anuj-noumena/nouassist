using NouAssist.RequirementsManagement.Specification.Enums;

namespace NouAssist.RequirementsManagement.Specification.Exceptions
{
    /// <summary>
    /// Exception thrown when an invalid status transition is attempted
    /// </summary>
    public class InvalidRequirementStatusTransitionException : RequirementSpecificationManagementDomainException
    {
        public InvalidRequirementStatusTransitionException(RequirementStatus fromStatus, RequirementStatus toStatus)
            : base("RequirementSpecificationManagement:ErrorMessage:InvalidStatusTransition")
        {
            WithData("fromStatus", fromStatus);
            WithData("toStatus", toStatus);
        }
    }
}
