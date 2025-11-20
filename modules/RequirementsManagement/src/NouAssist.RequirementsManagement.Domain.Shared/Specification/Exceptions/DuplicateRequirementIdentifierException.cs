namespace NouAssist.RequirementsManagement.Specification.Exceptions
{
    /// <summary>
    /// Exception thrown when a requirement with the same identifier already exists
    /// </summary>
    public class DuplicateRequirementIdentifierException : RequirementSpecificationManagementDomainException
    {
        public DuplicateRequirementIdentifierException(string identifier)
            : base("RequirementSpecificationManagement:ErrorMessage:DuplicateRequirementIdentifier")
        {
            WithData("identifier", identifier);
        }
    }
}
