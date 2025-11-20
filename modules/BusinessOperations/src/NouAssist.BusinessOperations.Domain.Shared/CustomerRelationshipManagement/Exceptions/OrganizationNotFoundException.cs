namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Exceptions
{
    /// <summary>
    /// Exception thrown when an organization cannot be found
    /// </summary>
    public class OrganizationNotFoundException : CRMDomainException
    {
        public OrganizationNotFoundException(Guid id)
            : base("CRM:ErrorMessage:OrganizationNotFound")
        {
            WithData("organizationId", id);
        }
    }
}
