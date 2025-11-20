namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Interfaces.Services
{
    /// <summary>
    /// Defines methods for managing portfolios
    /// </summary>
    public interface IPortfolioManager
    {
        /// <summary>
        /// Creates a new portfolio
        /// </summary>
        Task<PortfolioAggregate> CreatePortfolioAsync(
            string name,
            string description,
            Guid? tenantId = null);

        /// <summary>
        /// Adds an item to a portfolio
        /// </summary>
        Task<PortfolioItem> AddPortfolioItemAsync(
            Guid portfolioId,
            Guid itemId,
            PortfolioItemType itemType,
            string itemName,
            Guid? categoryId = null);

        /// <summary>
        /// Creates a category in a portfolio
        /// </summary>
        Task<PortfolioCategory> CreatePortfolioCategoryAsync(
            Guid portfolioId,
            string name,
            string description,
            string colorCode = null);

        /// <summary>
        /// Adds a risk to a portfolio
        /// </summary>
        Task<PortfolioRisk> AddPortfolioRiskAsync(
            Guid portfolioId,
            string title,
            RiskLevel level,
            string description,
            string mitigationPlan,
            decimal probability,
            decimal impact);

        /// <summary>
        /// Creates a sharing link for a portfolio
        /// </summary>
        Task<SharingLink> CreatePortfolioSharingLinkAsync(
            Guid portfolioId,
            DateTime expirationDate,
            bool canView,
            bool canEdit,
            bool canShare,
            bool canDelete);

        /// <summary>
        /// Gets portfolios with high risk
        /// </summary>
        Task<List<PortfolioAggregate>> GetHighRiskPortfoliosAsync(
            decimal riskScoreThreshold = 6.0m,
            Guid? tenantId = null);

        /// <summary>
        /// Gets portfolios containing a specific item
        /// </summary>
        Task<List<PortfolioAggregate>> GetPortfoliosContainingItemAsync(
            Guid itemId,
            PortfolioItemType itemType,
            Guid? tenantId = null);
    }
}
