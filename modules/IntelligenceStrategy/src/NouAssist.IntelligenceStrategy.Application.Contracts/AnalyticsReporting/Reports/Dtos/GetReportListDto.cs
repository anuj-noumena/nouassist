using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Reports.Dtos
{
    /// <summary>
    /// DTO for filtering reports
    /// </summary>
    public class GetReportListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// Text filter for name and description
        /// </summary>
        public string Filter { get; set; }
        
        /// <summary>
        /// Filter by category
        /// </summary>
        public string Category { get; set; }
        
        /// <summary>
        /// Filter by tag
        /// </summary>
        public string Tag { get; set; }
        
        /// <summary>
        /// Whether to show only reports created by the current user
        /// </summary>
        public bool? OnlyMine { get; set; }
        
        /// <summary>
        /// Whether to include details like parameters and schedules
        /// </summary>
        public bool IncludeDetails { get; set; } = false;
    }
}
