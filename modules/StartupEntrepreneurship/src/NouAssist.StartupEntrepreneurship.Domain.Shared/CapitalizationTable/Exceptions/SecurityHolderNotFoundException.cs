namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Exceptions
{
    /// <summary>
    /// Exception thrown when a security holder is not found
    /// </summary>
    public class SecurityHolderNotFoundException : CapTableDomainException
    {
        public SecurityHolderNotFoundException(Guid holderId)
            : base("CapTable:ErrorMessage:EntityNotFound")
        {
            WithData("entityType", "Security Holder");
            WithData("id", holderId);
        }
    }
}
