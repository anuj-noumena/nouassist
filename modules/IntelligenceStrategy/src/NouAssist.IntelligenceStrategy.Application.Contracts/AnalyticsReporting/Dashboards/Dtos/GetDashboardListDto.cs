using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Dashboards.Dtos
{
    /// <summary>
    /// DTO for filtering dashboards
    /// </summary>
    public class GetDashboardListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// Text filter for name and description
        /// </summary>
        public string Filter { get; set; }
        
        /// <summary>
        /// Whether to show only dashboards created by the current user
        /// </summary>
        public bool? OnlyMine { get; set; }
        
        /// <summary>
        /// Filter by tag
        /// </summary>
        public string Tag { get; set; }
        
        /// <summary>
        /// Whether to include the widgets in the results
        /// </summary>
        public bool IncludeWidgets { get; set; } = false;
    }
}
