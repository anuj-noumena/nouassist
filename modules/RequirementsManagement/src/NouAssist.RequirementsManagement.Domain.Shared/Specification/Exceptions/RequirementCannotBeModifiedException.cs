namespace NouAssist.RequirementsManagement.Specification.Exceptions
{
    /// <summary>
    /// Exception thrown when a requirement cannot be modified in its current state
    /// </summary>
    public class RequirementCannotBeModifiedException : RequirementSpecificationManagementDomainException
    {
        public RequirementCannotBeModifiedException(Guid id, RequirementStatus status)
            : base("RequirementSpecificationManagement:ErrorMessage:RequirementCannotBeModified")
        {
            WithData("requirementId", id);
            WithData("status", status);
        }
    }
}
