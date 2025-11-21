using System;

namespace NouAssist.RequirementsManagement.Specification.Exceptions
{
    /// <summary>
    /// Exception thrown when a requirement is not found
    /// </summary>
    public class RequirementNotFoundException : RequirementSpecificationManagementDomainException
    {
        public RequirementNotFoundException(Guid id)
            : base("RequirementSpecificationManagement:ErrorMessage:RequirementNotFound")
        {
            WithData("requirementId", id);
        }

        public RequirementNotFoundException(string identifier)
            : base("RequirementSpecificationManagement:ErrorMessage:RequirementNotFoundByIdentifier")
        {
            WithData("identifier", identifier);
        }
    }
}
