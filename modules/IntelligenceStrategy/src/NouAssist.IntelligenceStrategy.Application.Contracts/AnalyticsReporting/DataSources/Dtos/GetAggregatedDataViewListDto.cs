using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.DataSources.Dtos
{
    /// <summary>
    /// DTO for filtering aggregated data views
    /// </summary>
    public class GetAggregatedDataViewListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// Text filter for name and description
        /// </summary>
        public string Filter { get; set; }
        
        /// <summary>
        /// Filter by source data source ID
        /// </summary>
        public string SourceDataSourceId { get; set; }
        
        /// <summary>
        /// Filter by validation status
        /// </summary>
        public string ValidationStatus { get; set; }
    }
}
