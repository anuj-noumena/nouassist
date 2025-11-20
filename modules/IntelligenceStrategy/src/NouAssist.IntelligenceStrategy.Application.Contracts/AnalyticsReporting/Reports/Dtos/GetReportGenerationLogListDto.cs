using Volo.Abp.Application.Dtos;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Reports.Dtos
{
    /// <summary>
    /// DTO for filtering report generation logs
    /// </summary>
    public class GetReportGenerationLogListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// Filter by schedule ID
        /// </summary>
        public string ScheduleId { get; set; }
        
        /// <summary>
        /// Filter by status
        /// </summary>
        public string Status { get; set; }
        
        /// <summary>
        /// Filter by earliest start time
        /// </summary>
        public string StartTimeMin { get; set; }
        
        /// <summary>
        /// Filter by latest start time
        /// </summary>
        public string StartTimeMax { get; set; }
    }
}
