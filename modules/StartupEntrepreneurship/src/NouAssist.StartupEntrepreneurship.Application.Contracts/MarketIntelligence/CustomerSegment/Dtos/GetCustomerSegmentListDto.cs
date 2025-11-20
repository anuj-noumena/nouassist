using NouAssist.StartupEntrepreneurship.MarketIntelligence.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.CustomerSegment.Dtos
{
    /// <summary>
    /// DTO for getting a list of customer segment profiles with filtering options
    /// </summary>
    public class GetCustomerSegmentListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// Text to filter segments by name or description
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// Filter by segment type
        /// </summary>
        public SegmentType? Type { get; set; }

        /// <summary>
        /// Filter for active segments only
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// Filter by demographic age range
        /// </summary>
        public string AgeRange { get; set; }

        /// <summary>
        /// Filter by demographic income
        /// </summary>
        public string Income { get; set; }

        /// <summary>
        /// Filter by demographic location
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Include characteristics in the results
        /// </summary>
        public bool IncludeCharacteristics { get; set; } = false;
    }
}
