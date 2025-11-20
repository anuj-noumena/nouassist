using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.SwotAnalysis.Dtos
{
    /// <summary>
    /// DTO for getting a list of SWOT analyses with filtering options
    /// </summary>
    public class GetSwotAnalysisListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// Text to filter analyses by title or description
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// Filter for completed analyses only
        /// </summary>
        public bool? IsCompleted { get; set; }

        /// <summary>
        /// Filter for analyses created after this date
        /// </summary>
        public DateTime? CreatedAfter { get; set; }

        /// <summary>
        /// Filter for analyses created before this date
        /// </summary>
        public DateTime? CreatedBefore { get; set; }

        /// <summary>
        /// Include SWOT items in the results
        /// </summary>
        public bool IncludeItems { get; set; } = false;
    }
}
