using System.ComponentModel.DataAnnotations;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.CustomerSegment.Dtos
{
    /// <summary>
    /// DTO for creating demographic information
    /// </summary>
    public class CreateDemographicsDto
    {
        /// <summary>
        /// Age range
        /// </summary>
        [StringLength(MarketIntelligenceConsts.CustomerSegment.MaxDemographicValueLength)]
        public string AgeRange { get; set; }

        /// <summary>
        /// Gender distribution
        /// </summary>
        [StringLength(MarketIntelligenceConsts.CustomerSegment.MaxDemographicValueLength)]
        public string Gender { get; set; }

        /// <summary>
        /// Income level
        /// </summary>
        [StringLength(MarketIntelligenceConsts.CustomerSegment.MaxDemographicValueLength)]
        public string Income { get; set; }

        /// <summary>
        /// Education level
        /// </summary>
        [StringLength(MarketIntelligenceConsts.CustomerSegment.MaxDemographicValueLength)]
        public string Education { get; set; }

        /// <summary>
        /// Geographic location
        /// </summary>
        [StringLength(MarketIntelligenceConsts.CustomerSegment.MaxDemographicValueLength)]
        public string Location { get; set; }

        /// <summary>
        /// Typical occupation
        /// </summary>
        [StringLength(MarketIntelligenceConsts.CustomerSegment.MaxDemographicValueLength)]
        public string Occupation { get; set; }
    }
}
