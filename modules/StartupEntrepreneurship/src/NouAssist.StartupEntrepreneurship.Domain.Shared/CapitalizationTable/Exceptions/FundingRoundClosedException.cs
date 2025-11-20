namespace NouAssist.StartupEntrepreneurship.CapitalizationTable.Exceptions
{
    /// <summary>
    /// Exception thrown when attempting to modify a closed funding round
    /// </summary>
    public class FundingRoundClosedException : CapTableDomainException
    {
        public FundingRoundClosedException(Guid fundingRoundId)
            : base("CapTable:ErrorMessage:FundingRoundClosed")
        {
            WithData("fundingRoundId", fundingRoundId);
        }
    }
}
