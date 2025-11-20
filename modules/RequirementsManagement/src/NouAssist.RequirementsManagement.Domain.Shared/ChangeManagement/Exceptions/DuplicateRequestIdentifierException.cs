namespace NouAssist.RequirementsManagement.ChangeManagement.Exceptions
{
    /// <summary>
    /// Exception thrown when a change request with the same identifier already exists
    /// </summary>
    public class DuplicateRequestIdentifierException : ChangeManagementDomainException
    {
        public DuplicateRequestIdentifierException(string requestIdentifier)
            : base("ChangeManagement:ErrorMessage:DuplicateRequestIdentifier")
        {
            WithData("requestIdentifier", requestIdentifier);
        }
    }
}
