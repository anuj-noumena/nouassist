namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Events
{
    /// <summary>
    /// Event published when a contract is created
    /// </summary>
    [EventName("NouAssist.BusinessOperations.ContractPortfolioManagement.ContractCreated")]
    public class ContractCreatedEto : ContractEventBase
    {
        public Guid DocumentId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
