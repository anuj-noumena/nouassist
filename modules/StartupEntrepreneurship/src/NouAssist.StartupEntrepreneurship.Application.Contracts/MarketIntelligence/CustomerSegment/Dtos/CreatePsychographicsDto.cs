using System.ComponentModel.DataAnnotations;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.CustomerSegment.Dtos
{
    /// <summary>
    /// DTO for creating psychographic information
    /// </summary>
    public class CreatePsychographicsDto
    {
        /// <summary>
        /// Interests and hobbies
        /// </summary>
        [StringLength(MarketIntelligenceConsts.CustomerSegment.MaxPsychographicValueLength)]
        public string Interests { get; set; }

        /// <summary>
        /// Core values
        /// </summary>
        [StringLength(MarketIntelligenceConsts.CustomerSegment.MaxPsychographicValueLength)]
        public string Values { get; set; }

        /// <summary>
        /// Lifestyle description
        /// </summary>
        [StringLength(MarketIntelligenceConsts.CustomerSegment.MaxPsychographicValueLength)]
        public string Lifestyle { get; set; }

        /// <summary>
        /// Personality traits
        /// </summary>
        [StringLength(MarketIntelligenceConsts.CustomerSegment.MaxPsychographicValueLength)]
        public string Personality { get; set; }

        /// <summary>
        /// Attitudes and beliefs
        /// </summary>
        [StringLength(MarketIntelligenceConsts.CustomerSegment.MaxPsychographicValueLength)]
        public string Attitudes { get; set; }
    }
}
