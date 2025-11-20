namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Exceptions
{
    /// <summary>
    /// Exception thrown when attempting to create a lead or contact with an email that already exists
    /// </summary>
    public class DuplicateEmailException : CRMDomainException
    {
        public DuplicateEmailException(string email, string entityType)
            : base("CRM:ErrorMessage:DuplicateEmail")
        {
            WithData("email", email);
            WithData("entityType", entityType);
        }
    }
}
