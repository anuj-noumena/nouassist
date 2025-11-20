using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.ProjectExecution.TimeLogs.Dtos
{
    /// <summary>
    /// DTO for time log summary
    /// </summary>
    public class TimeLogSummaryDto : EntityDto
    {
        /// <summary>
        /// Start date of the summary period
        /// </summary>
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// End date of the summary period
        /// </summary>
        public DateTime EndDate { get; set; }
        
        /// <summary>
        /// Total hours logged in the period
        /// </summary>
        public double TotalHours { get; set; }
        
        /// <summary>
        /// Hours broken down by project
        /// </summary>
        public List<TimeLogProjectSummaryDto> ProjectBreakdown { get; set; }
        
        /// <summary>
        /// Hours broken down by user
        /// </summary>
        public List<TimeLogUserSummaryDto> UserBreakdown { get; set; }
        
        /// <summary>
        /// Hours broken down by day
        /// </summary>
        public List<TimeLogDailySummaryDto> DailyBreakdown { get; set; }
    }
}
using System;
using System.Collections.Generic;

namespace NouAssist.BusinessOperations.ProjectExecution.TimeLogs.Dtos
{
    /// <summary>
    /// DTO for time log summary information
    /// </summary>
    public class TimeLogSummaryDto
    {
        /// <summary>
        /// Total hours logged
        /// </summary>
        public double TotalHours { get; set; }
        
        /// <summary>
        /// Hours grouped by date
        /// </summary>
        public Dictionary<DateTime, double> HoursByDate { get; set; }
        
        /// <summary>
        /// Hours grouped by user
        /// </summary>
        public Dictionary<Guid, double> HoursByUser { get; set; }
        
        /// <summary>
        /// Hours grouped by task
        /// </summary>
        public Dictionary<Guid, double> HoursByTask { get; set; }
        
        /// <summary>
        /// Creates a new instance of TimeLogSummaryDto
        /// </summary>
        public TimeLogSummaryDto()
        {
            HoursByDate = new Dictionary<DateTime, double>();
            HoursByUser = new Dictionary<Guid, double>();
            HoursByTask = new Dictionary<Guid, double>();
        }
    }
}
