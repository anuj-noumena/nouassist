namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Exceptions
{
    /// <summary>
    /// Exception thrown when a portfolio is not found
    /// </summary>
    public class PortfolioNotFoundException : ContractPortfolioManagementDomainException
    {
        public Guid PortfolioId { get; }

        public PortfolioNotFoundException(Guid portfolioId)
            : base("ContractPortfolioManagement:ErrorMessage:PortfolioNotFound")
        {
            PortfolioId = portfolioId;
            WithData("portfolioId", portfolioId);
        }
    }
}
