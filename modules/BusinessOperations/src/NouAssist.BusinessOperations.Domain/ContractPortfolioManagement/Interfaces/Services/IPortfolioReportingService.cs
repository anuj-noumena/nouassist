namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Interfaces.Services
{
    /// <summary>
    /// Defines methods for portfolio reporting
    /// </summary>
    public interface IPortfolioReportingService
    {
        /// <summary>
        /// Gets performance metrics for a portfolio
        /// </summary>
        Task<List<PerformanceMetric>> GetPortfolioMetricsAsync(Guid portfolioId);

        /// <summary>
        /// Gets risk assessment for a portfolio
        /// </summary>
        Task<List<PortfolioRisk>> GetPortfolioRisksAsync(Guid portfolioId);

        /// <summary>
        /// Gets compliance status for a portfolio
        /// </summary>
        Task<List<PortfolioComplianceRecord>> GetPortfolioComplianceStatusAsync(Guid portfolioId);

        /// <summary>
        /// Gets overall portfolio statistics
        /// </summary>
        Task<PortfolioStatistics> GetPortfolioStatisticsAsync(Guid portfolioId);
    }
}
