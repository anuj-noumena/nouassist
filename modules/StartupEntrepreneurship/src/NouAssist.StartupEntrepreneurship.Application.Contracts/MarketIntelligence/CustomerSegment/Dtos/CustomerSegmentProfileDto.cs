using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.CustomerSegment.Dtos
{
    /// <summary>
    /// DTO for customer segment profile data
    /// </summary>
    public class CustomerSegmentProfileDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// Segment name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Segment description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Segment type
        /// </summary>
        public SegmentType Type { get; set; }

        /// <summary>
        /// Demographic information
        /// </summary>
        public DemographicsDto Demographics { get; set; }

        /// <summary>
        /// Psychographic information
        /// </summary>
        public PsychographicsDto Psychographics { get; set; }

        /// <summary>
        /// Estimated segment size
        /// </summary>
        public decimal EstimatedSize { get; set; }

        /// <summary>
        /// Estimated segment value
        /// </summary>
        public decimal EstimatedValue { get; set; }

        /// <summary>
        /// Primary needs of the segment
        /// </summary>
        public string PrimaryNeeds { get; set; }

        /// <summary>
        /// Pain points of the segment
        /// </summary>
        public string PainPoints { get; set; }

        /// <summary>
        /// Preferred communication channels
        /// </summary>
        public string PreferredChannels { get; set; }

        /// <summary>
        /// Whether the segment is active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Segment characteristics
        /// </summary>
        public List<SegmentCharacteristicsDto> Characteristics { get; set; }
    }
}
