namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Entities.Portfolio
{
    /// <summary>
    /// Represents a risk associated with a portfolio
    /// </summary>
    public class PortfolioRisk : Entity<Guid>
    {
        public Guid PortfolioId { get; private set; }
        public string Title { get; private set; }
        public RiskAssessmentData Assessment { get; private set; }

        protected PortfolioRisk() { }

        public PortfolioRisk(
            Guid id,
            Guid portfolioId,
            string title,
            RiskAssessmentData assessment)
            : base(id)
        {
            PortfolioId = portfolioId;
            Title = Check.NotNullOrWhiteSpace(title, nameof(title));
            Assessment = assessment;
        }

        public void UpdateTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(title, nameof(title));
        }

        public void UpdateAssessment(RiskAssessmentData assessment)
        {
            Assessment = assessment;
        }
    }
}
