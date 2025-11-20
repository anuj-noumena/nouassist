using System.ComponentModel.DataAnnotations;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.MarketTrend.Dtos
{
    /// <summary>
    /// DTO for creating a trend data source
    /// </summary>
    public class CreateTrendDataSourceDto
    {
        /// <summary>
        /// Name of the source
        /// </summary>
        [Required]
        [StringLength(MarketIntelligenceConsts.MarketTrend.MaxTitleLength)]
        public string SourceName { get; set; }

        /// <summary>
        /// URL of the source
        /// </summary>
        [Required]
        [StringLength(MarketIntelligenceConsts.MarketTrend.MaxSourceUrlLength)]
        [RegularExpression(@"^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$", 
            ErrorMessage = "Please enter a valid URL")]
        public string SourceUrl { get; set; }

        /// <summary>
        /// Type of source (research, news, etc.)
        /// </summary>
        [StringLength(100)]
        public string SourceType { get; set; }

        /// <summary>
        /// Reliability score of the source (0-1 scale)
        /// </summary>
        [Range(0.0, 1.0, ErrorMessage = "Reliability must be between 0 and 1")]
        public decimal Reliability { get; set; } = 0.5m;

        /// <summary>
        /// Data quality assessment
        /// </summary>
        [StringLength(200)]
        public string DataQuality { get; set; }

        /// <summary>
        /// Extracted data
        /// </summary>
        public string ExtractedData { get; set; }
    }
}
