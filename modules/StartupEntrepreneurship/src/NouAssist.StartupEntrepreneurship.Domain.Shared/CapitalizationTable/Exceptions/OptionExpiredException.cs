namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Exceptions
{
    /// <summary>
    /// Exception thrown when attempting to exercise an expired option
    /// </summary>
    public class OptionExpiredException : CapTableDomainException
    {
        public OptionExpiredException(Guid optionId, DateTime expiredDate)
            : base("CapTable:ErrorMessage:OptionExpired")
        {
            WithData("optionId", optionId);
            WithData("expiredDate", expiredDate);
        }
    }
}
