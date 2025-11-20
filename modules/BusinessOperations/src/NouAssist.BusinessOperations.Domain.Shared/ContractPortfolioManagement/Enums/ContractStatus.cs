namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Enums
{
    /// <summary>
    /// Represents the status of a contract
    /// </summary>
    public enum ContractStatus
    {
        Draft = 0,
        UnderReview = 10,
        Negotiation = 20,
        PendingSignature = 30,
        Active = 40,
        Expired = 50,
        Terminated = 60,
        Archived = 70
    }
}
