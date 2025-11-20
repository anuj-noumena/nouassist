namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Competitor.Dtos
{
    /// <summary>
    /// DTO for competitor information
    /// </summary>
    public class CompetitorInfoDto
    {
        /// <summary>
        /// Competitor's website URL
        /// </summary>
        public string Website { get; set; }

        /// <summary>
        /// Industry the competitor operates in
        /// </summary>
        public string Industry { get; set; }

        /// <summary>
        /// Geographic location of the competitor
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Approximate number of employees
        /// </summary>
        public int? EmployeeCount { get; set; }

        /// <summary>
        /// Total funding amount received
        /// </summary>
        public decimal? FundingAmount { get; set; }

        /// <summary>
        /// Funding source information
        /// </summary>
        public string FundingSource { get; set; }
    }
}
