using System;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.Reports.Dtos
{
    /// <summary>
    /// DTO for schedule timing
    /// </summary>
    public class ScheduleTimingDto
    {
        /// <summary>
        /// Cron expression for the schedule
        /// </summary>
        public string CronExpression { get; set; }
        
        /// <summary>
        /// Timezone for the schedule
        /// </summary>
        public string TimeZone { get; set; }
        
        /// <summary>
        /// Start date for the schedule
        /// </summary>
        public DateTime? StartDate { get; set; }
        
        /// <summary>
        /// End date for the schedule
        /// </summary>
        public DateTime? EndDate { get; set; }
        
        /// <summary>
        /// Whether the schedule is active
        /// </summary>
        public bool IsActive { get; set; }
    }
}
