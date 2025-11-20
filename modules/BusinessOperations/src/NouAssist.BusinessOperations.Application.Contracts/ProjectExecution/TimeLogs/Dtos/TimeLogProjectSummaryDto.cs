using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.ProjectExecution.TimeLogs.Dtos
{
    /// <summary>
    /// DTO for time log summary by project
    /// </summary>
    public class TimeLogProjectSummaryDto : EntityDto
    {
        /// <summary>
        /// ID of the project
        /// </summary>
        public Guid ProjectId { get; set; }
        
        /// <summary>
        /// Name of the project
        /// </summary>
        public string ProjectName { get; set; }
        
        /// <summary>
        /// Total hours logged for this project
        /// </summary>
        public double Hours { get; set; }
        
        /// <summary>
        /// Percentage of total hours
        /// </summary>
        public double Percentage { get; set; }
    }
}
