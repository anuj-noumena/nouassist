namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Exceptions
{
    /// <summary>
    /// Exception thrown when attempting to create a duplicate security holder
    /// </summary>
    public class DuplicateSecurityHolderException : CapTableDomainException
    {
        public DuplicateSecurityHolderException(string holderName)
            : base("CapTable:ErrorMessage:DuplicateSecurityHolder")
        {
            WithData("holderName", holderName);
        }
    }
}
