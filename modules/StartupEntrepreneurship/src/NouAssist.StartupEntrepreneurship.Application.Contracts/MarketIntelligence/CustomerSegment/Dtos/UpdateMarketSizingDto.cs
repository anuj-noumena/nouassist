using System.ComponentModel.DataAnnotations;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.CustomerSegment.Dtos
{
    /// <summary>
    /// DTO for updating market sizing information
    /// </summary>
    public class UpdateMarketSizingDto
    {
        /// <summary>
        /// Estimated size of the segment (number of customers, etc.)
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Estimated size must be a positive number")]
        public decimal EstimatedSize { get; set; }

        /// <summary>
        /// Estimated value of the segment (revenue potential, etc.)
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Estimated value must be a positive number")]
        public decimal EstimatedValue { get; set; }
    }
}
