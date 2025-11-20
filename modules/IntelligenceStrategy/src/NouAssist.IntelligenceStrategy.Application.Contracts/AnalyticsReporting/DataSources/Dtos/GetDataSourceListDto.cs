using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.DataSources.Dtos
{
    /// <summary>
    /// DTO for filtering data sources
    /// </summary>
    public class GetDataSourceListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// Text filter for name and description
        /// </summary>
        public string Filter { get; set; }
        
        /// <summary>
        /// Filter by data source type
        /// </summary>
        public string Type { get; set; }
        
        /// <summary>
        /// Filter by tag
        /// </summary>
        public string Tag { get; set; }
        
        /// <summary>
        /// Whether to include only active data sources
        /// </summary>
        public bool? OnlyActive { get; set; }
        
        /// <summary>
        /// Whether to include transformation steps in the results
        /// </summary>
        public bool IncludeTransformations { get; set; } = false;
    }
}
