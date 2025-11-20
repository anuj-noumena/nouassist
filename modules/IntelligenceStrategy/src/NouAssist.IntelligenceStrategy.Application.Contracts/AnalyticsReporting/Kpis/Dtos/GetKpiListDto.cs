using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Kpis.Dtos
{
    /// <summary>
    /// DTO for filtering KPIs
    /// </summary>
    public class GetKpiListDto : PagedAndSortedResultRequestDto
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
        /// Filter by status (OK, Warning, Critical, etc.)
        /// </summary>
        public string Status { get; set; }
        
        /// <summary>
        /// Whether to include only active KPIs
        /// </summary>
        public bool? OnlyActive { get; set; }
        
        /// <summary>
        /// Whether to include details like thresholds
        /// </summary>
        public bool IncludeDetails { get; set; } = false;
    }
}
