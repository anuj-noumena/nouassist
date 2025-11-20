namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Events
{
    /// <summary>
    /// Event published when a contract status is changed
    /// </summary>
    [EventName("NouAssist.BusinessOperations.ContractPortfolioManagement.ContractStatusChanged")]
    public class ContractStatusChangedEto : ContractEventBase
    {
        public ContractStatus PreviousStatus { get; set; }
        public DateTime StatusChangeDate { get; set; }
    }
}
