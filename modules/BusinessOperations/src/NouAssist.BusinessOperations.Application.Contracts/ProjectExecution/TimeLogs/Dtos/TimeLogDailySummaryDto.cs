using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.ProjectExecution.TimeLogs.Dtos
{
    /// <summary>
    /// DTO for time log summary by day
    /// </summary>
    public class TimeLogDailySummaryDto : EntityDto
    {
        /// <summary>
        /// Date for this summary
        /// </summary>
        public DateTime Date { get; set; }
        
        /// <summary>
        /// Total hours logged on this day
        /// </summary>
        public double Hours { get; set; }
    }
}
