namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.CustomerSegment.Dtos
{
    /// <summary>
    /// DTO for demographic information
    /// </summary>
    public class DemographicsDto
    {
        /// <summary>
        /// Age range
        /// </summary>
        public string AgeRange { get; set; }

        /// <summary>
        /// Gender distribution
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Income level
        /// </summary>
        public string Income { get; set; }

        /// <summary>
        /// Education level
        /// </summary>
        public string Education { get; set; }

        /// <summary>
        /// Geographic location
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Typical occupation
        /// </summary>
        public string Occupation { get; set; }
    }
}
