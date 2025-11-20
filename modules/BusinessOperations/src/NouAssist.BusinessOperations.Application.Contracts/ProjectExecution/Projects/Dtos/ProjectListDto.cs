using System;
using System.Collections.Generic;
using NouAssist.BusinessOperations.ProjectExecution.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.ProjectExecution.Projects.Dtos
{
    /// <summary>
    /// DTO for project list items
    /// </summary>
    public class ProjectListDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// Name of the project
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Description of the project
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Start date of the project
        /// </summary>
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// Planned end date of the project
        /// </summary>
        public DateTime PlannedEndDate { get; set; }
        
        /// <summary>
        /// Current status of the project
        /// </summary>
        public ProjectStatus Status { get; set; }
        
        /// <summary>
        /// Completion percentage of the project
        /// </summary>
        public double CompletionPercentage { get; set; }
        
        /// <summary>
        /// Number of active tasks in the project
        /// </summary>
        public int ActiveTaskCount { get; set; }
        
        /// <summary>
        /// Number of completed tasks in the project
        /// </summary>
        public int CompletedTaskCount { get; set; }
    }
}
