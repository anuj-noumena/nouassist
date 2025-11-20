namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Exceptions
{
    /// <summary>
    /// Exception thrown when attempting to add a duplicate item to a portfolio
    /// </summary>
    public class DuplicatePortfolioItemException : ContractPortfolioManagementDomainException
    {
        public Guid PortfolioId { get; }
        public Guid ItemId { get; }
        public PortfolioItemType ItemType { get; }

        public DuplicatePortfolioItemException(Guid portfolioId, Guid itemId, PortfolioItemType itemType)
            : base("ContractPortfolioManagement:ErrorMessage:DuplicatePortfolioItem")
        {
            PortfolioId = portfolioId;
            ItemId = itemId;
            ItemType = itemType;
            WithData("portfolioId", portfolioId);
            WithData("itemId", itemId);
            WithData("itemType", itemType);
        }
    }
}
