using System.ComponentModel.DataAnnotations;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.CustomerSegment.Dtos
{
    /// <summary>
    /// DTO for updating a customer segment profile
    /// </summary>
    public class UpdateCustomerSegmentProfileDto
    {
        /// <summary>
        /// Segment name
        /// </summary>
        [Required]
        [StringLength(MarketIntelligenceConsts.CustomerSegment.MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// Segment description
        /// </summary>
        [Required]
        [StringLength(MarketIntelligenceConsts.CustomerSegment.MaxDescriptionLength)]
        public string Description { get; set; }

        /// <summary>
        /// Demographic information
        /// </summary>
        [Required]
        public CreateDemographicsDto Demographics { get; set; }

        /// <summary>
        /// Psychographic information
        /// </summary>
        [Required]
        public CreatePsychographicsDto Psychographics { get; set; }

        /// <summary>
        /// Primary needs of the segment
        /// </summary>
        [Required]
        [StringLength(MarketIntelligenceConsts.CustomerSegment.MaxNeedDescriptionLength)]
        public string PrimaryNeeds { get; set; }

        /// <summary>
        /// Pain points of the segment
        /// </summary>
        [StringLength(MarketIntelligenceConsts.CustomerSegment.MaxNeedDescriptionLength)]
        public string PainPoints { get; set; }

        /// <summary>
        /// Preferred communication channels
        /// </summary>
        [StringLength(MarketIntelligenceConsts.CustomerSegment.MaxNeedDescriptionLength)]
        public string PreferredChannels { get; set; }
    }
}
