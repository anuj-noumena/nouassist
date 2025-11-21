using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Constants;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Competitor.Dtos
{
    /// <summary>
    /// DTO for creating competitor information
    /// </summary>
    public class CreateCompetitorInfoDto
    {
        /// <summary>
        /// Competitor's website URL
        /// </summary>
        [StringLength(MarketIntelligenceConsts.Competitor.MaxWebsiteLength)]
        [RegularExpression(@"^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$", 
            ErrorMessage = "Please enter a valid URL")]
        public string Website { get; set; }

        /// <summary>
        /// Industry the competitor operates in
        /// </summary>
        [StringLength(MarketIntelligenceConsts.Competitor.MaxIndustryLength)]
        public string Industry { get; set; }

        /// <summary>
        /// Geographic location of the competitor
        /// </summary>
        [StringLength(MarketIntelligenceConsts.Competitor.MaxLocationLength)]
        public string Location { get; set; }

        /// <summary>
        /// Approximate number of employees
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Employee count must be a positive number")]
        public int? EmployeeCount { get; set; }

        /// <summary>
        /// Total funding amount received
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Funding amount must be a positive number")]
        public decimal? FundingAmount { get; set; }

        /// <summary>
        /// Funding source information
        /// </summary>
        [StringLength(MarketIntelligenceConsts.Competitor.MaxFundingSourceLength)]
        public string FundingSource { get; set; }
    }
}
