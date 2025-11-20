using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.ProjectExecution.TimeLogs.Dtos
{
    /// <summary>
    /// DTO for time log summary by user
    /// </summary>
    public class TimeLogUserSummaryDto : EntityDto
    {
        /// <summary>
        /// ID of the user
        /// </summary>
        public Guid UserId { get; set; }
        
        /// <summary>
        /// Name of the user
        /// </summary>
        public string UserName { get; set; }
        
        /// <summary>
        /// Total hours logged by this user
        /// </summary>
        public double Hours { get; set; }
        
        /// <summary>
        /// Percentage of total hours
        /// </summary>
        public double Percentage { get; set; }
    }
}
