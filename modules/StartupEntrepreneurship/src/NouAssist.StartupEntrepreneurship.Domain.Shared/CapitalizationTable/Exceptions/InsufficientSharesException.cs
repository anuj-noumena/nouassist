namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Exceptions
{
    /// <summary>
    /// Exception thrown when there are insufficient shares for an operation
    /// </summary>
    public class InsufficientSharesException : CapTableDomainException
    {
        public InsufficientSharesException(decimal requested, decimal available)
            : base("CapTable:ErrorMessage:InsufficientShares")
        {
            WithData("requested", requested);
            WithData("available", available);
        }
    }
}
