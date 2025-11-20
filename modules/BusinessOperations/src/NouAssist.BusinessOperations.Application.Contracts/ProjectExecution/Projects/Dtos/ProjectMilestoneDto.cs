using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.ProjectExecution.Projects.Dtos
{
    /// <summary>
    /// DTO for ProjectMilestone entity
    /// </summary>
    public class ProjectMilestoneDto : EntityDto
    {
        /// <summary>
        /// ID of the project this milestone belongs to
        /// </summary>
        public Guid ProjectId { get; set; }
        
        /// <summary>
        /// Name of the milestone
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Target date for the milestone
        /// </summary>
        public DateTime TargetDate { get; set; }
        
        /// <summary>
        /// Optional description of the milestone
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Completion date of the milestone, if completed
        /// </summary>
        public DateTime? CompletedDate { get; set; }
        
        /// <summary>
        /// Indicates if the milestone is completed
        /// </summary>
        public bool IsCompleted { get; set; }
    }
}
using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.ProjectExecution.Projects.Dtos
{
    /// <summary>
    /// DTO for ProjectMilestone entity
    /// </summary>
    public class ProjectMilestoneDto : EntityDto
    {
        /// <summary>
        /// ID of the project this milestone belongs to
        /// </summary>
        public Guid ProjectId { get; set; }
        
        /// <summary>
        /// Name of the milestone
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Target date for the milestone
        /// </summary>
        public DateTime TargetDate { get; set; }
        
        /// <summary>
        /// Optional description of the milestone
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Completion date of the milestone, if completed
        /// </summary>
        public DateTime? CompletedDate { get; set; }
        
        /// <summary>
        /// Indicates if the milestone is completed
        /// </summary>
        public bool IsCompleted { get; set; }
    }
}
