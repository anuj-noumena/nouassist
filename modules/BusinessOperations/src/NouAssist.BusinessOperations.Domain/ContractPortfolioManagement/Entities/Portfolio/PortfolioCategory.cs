namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Entities.Portfolio
{
    /// <summary>
    /// Represents a category for organizing portfolio items
    /// </summary>
    public class PortfolioCategory : Entity<Guid>
    {
        public Guid PortfolioId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string ColorCode { get; private set; }

        protected PortfolioCategory() { }

        public PortfolioCategory(
            Guid id,
            Guid portfolioId,
            string name,
            string description,
            string colorCode = null)
            : base(id)
        {
            PortfolioId = portfolioId;
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: ContractPortfolioManagementConsts.PortfolioCategory.MaxNameLength);
            
            Description = Check.Length(
                description,
                nameof(description),
                maxLength: ContractPortfolioManagementConsts.PortfolioCategory.MaxDescriptionLength);
            
            ColorCode = colorCode;
        }

        public void Update(
            string name,
            string description,
            string colorCode = null)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: ContractPortfolioManagementConsts.PortfolioCategory.MaxNameLength);
            
            Description = Check.Length(
                description,
                nameof(description),
                maxLength: ContractPortfolioManagementConsts.PortfolioCategory.MaxDescriptionLength);
            
            ColorCode = colorCode;
        }
    }
}
