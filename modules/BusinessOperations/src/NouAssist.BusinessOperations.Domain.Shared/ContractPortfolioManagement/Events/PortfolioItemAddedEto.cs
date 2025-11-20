namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Events
{
    /// <summary>
    /// Event published when an item is added to a portfolio
    /// </summary>
    [EventName("NouAssist.BusinessOperations.ContractPortfolioManagement.PortfolioItemAdded")]
    public class PortfolioItemAddedEto
    {
        public Guid PortfolioId { get; set; }
        public Guid ItemId { get; set; }
        public PortfolioItemType ItemType { get; set; }
        public string ItemName { get; set; }
        public Guid? TenantId { get; set; }
    }
}
