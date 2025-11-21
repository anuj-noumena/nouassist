using System;
using System.Collections.Generic;
using NouAssist.BusinessOperations.ProjectExecution.Enums;
using NouAssist.BusinessOperations.ProjectExecution.Projects.Dtos;
using NouAssist.BusinessOperations.ProjectExecution.Tasks.Dtos;
using NouAssist.BusinessOperations.ProjectExecution.TimeLogs.Dtos;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.ProjectExecution.Reports.Dtos
{
    /// <summary>
    /// DTO for project report
    /// </summary>
    public class ProjectReportDto : EntityDto<Guid>
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
        /// Actual end date of the project, if completed
        /// </summary>
        public DateTime? ActualEndDate { get; set; }
        
        /// <summary>
        /// Completion percentage of the project
        /// </summary>
        public double CompletionPercentage { get; set; }
        
        /// <summary>
        /// Total hours estimated for the project
        /// </summary>
        public double TotalEstimatedHours { get; set; }
        
        /// <summary>
        /// Total hours logged for the project
        /// </summary>
        public double TotalLoggedHours { get; set; }
        
        /// <summary>
        /// Number of tasks in the project by status
        /// </summary>
        public Dictionary<TaskStatus, int> TaskCountByStatus { get; set; }
        
        /// <summary>
        /// Collection of project milestones
        /// </summary>
        public List<ProjectMilestoneDto> Milestones { get; set; }
        
        /// <summary>
        /// Hours logged by user
        /// </summary>
        public List<TimeLogUserSummaryDto> HoursByUser { get; set; }
        
        /// <summary>
        /// Hours logged by day
        /// </summary>
        public List<TimeLogDailySummaryDto> HoursByDay { get; set; }
        
        /// <summary>
        /// List of overdue tasks
        /// </summary>
        public List<TaskListDto> OverdueTasks { get; set; }
        
        /// <summary>
        /// List of upcoming milestones
        /// </summary>
        public List<ProjectMilestoneDto> UpcomingMilestones { get; set; }
    }
}
