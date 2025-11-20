using NouAssist.StartupEntrepreneurship.MarketIntelligence.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Competitor.Dtos
{
    /// <summary>
    /// DTO for getting a list of competitors with filtering options
    /// </summary>
    public class GetCompetitorListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// Text to filter competitors by name, description, or tags
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// Filter by competitor status
        /// </summary>
        public CompetitorStatus? Status { get; set; }

        /// <summary>
        /// Filter by competitor threat level
        /// </summary>
        public CompetitorThreatLevel? ThreatLevel { get; set; }

        /// <summary>
        /// Filter by industry
        /// </summary>
        public string Industry { get; set; }

        /// <summary>
        /// Maximum number of recent updates to include per competitor
        /// </summary>
        public int MaxRecentUpdates { get; set; } = 3;
    }
}
