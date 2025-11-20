namespace NouAssist.BusinessOperations.ContractPortfolioManagement.ValueObjects
{
    /// <summary>
    /// Value object representing a reference to an item (contract, project, etc.) in a portfolio
    /// </summary>
    public class ItemReference : ValueObject
    {
        public Guid ItemId { get; private set; }
        public PortfolioItemType ItemType { get; private set; }
        public string ItemName { get; private set; }

        protected ItemReference() { }

        public ItemReference(Guid itemId, PortfolioItemType itemType, string itemName)
        {
            ItemId = itemId;
            ItemType = itemType;
            ItemName = Check.NotNullOrWhiteSpace(itemName, nameof(itemName));
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return ItemId;
            yield return ItemType;
            yield return ItemName;
        }
    }
}
