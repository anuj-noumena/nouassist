using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Kpis.Dtos
{
    /// <summary>
    /// DTO for filtering KPI alerts
    /// </summary>
    public class GetKpiAlertListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// Filter by KPI ID
        /// </summary>
        public string KpiId { get; set; }
        
        /// <summary>
        /// Filter by status
        /// </summary>
        public string Status { get; set; }
        
        /// <summary>
        /// Filter by earliest timestamp
        /// </summary>
        public string TimestampMin { get; set; }
        
        /// <summary>
        /// Filter by latest timestamp
        /// </summary>
        public string TimestampMax { get; set; }
        
        /// <summary>
        /// Filter to only show alerts where notifications were sent
        /// </summary>
        public bool? NotificationSent { get; set; }
    }
}
