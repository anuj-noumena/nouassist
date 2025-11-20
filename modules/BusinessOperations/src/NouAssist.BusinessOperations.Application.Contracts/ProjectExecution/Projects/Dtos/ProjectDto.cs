using System;
using System.Collections.Generic;
using NouAssist.BusinessOperations.ProjectExecution.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.ProjectExecution.Projects.Dtos
{
    /// <summary>
    /// DTO for Project entity
    /// </summary>
    public class ProjectDto : AuditedEntityDto<Guid>
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
        /// Optional ID of the organization associated with the project
        /// </summary>
        public Guid? OrganizationId { get; set; }
        
        /// <summary>
        /// Optional ID of the contact associated with the project
        /// </summary>
        public Guid? ContactId { get; set; }
        
        /// <summary>
        /// Completion percentage of the project
        /// </summary>
        public double CompletionPercentage { get; set; }
        
        /// <summary>
        /// Collection of project phases
        /// </summary>
        public List<ProjectPhaseDto> Phases { get; set; }
        
        /// <summary>
        /// Collection of project milestones
        /// </summary>
        public List<ProjectMilestoneDto> Milestones { get; set; }
        
        /// <summary>
        /// Collection of project members
        /// </summary>
        public List<ProjectMemberDto> Members { get; set; }
    }
}
using System;
using System.Collections.Generic;
using NouAssist.BusinessOperations.ProjectExecution.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.ProjectExecution.Projects.Dtos
{
    /// <summary>
    /// DTO for Project entity
    /// </summary>
    public class ProjectDto : FullAuditedEntityDto<Guid>
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
        /// Status of the project
        /// </summary>
        public ProjectStatus Status { get; set; }
        
        /// <summary>
        /// String representation of the project status
        /// </summary>
        public string StatusString => Status.ToString();
        
        /// <summary>
        /// Actual end date of the project, if completed
        /// </summary>
        public DateTime? ActualEndDate { get; set; }
        
        /// <summary>
        /// ID of the organization associated with the project
        /// </summary>
        public Guid? OrganizationId { get; set; }
        
        /// <summary>
        /// ID of the contact associated with the project
        /// </summary>
        public Guid? ContactId { get; set; }
        
        /// <summary>
        /// Progress percentage of the project
        /// </summary>
        public double CompletionPercentage { get; set; }
        
        /// <summary>
        /// Total logged hours for the project
        /// </summary>
        public double TotalLoggedHours { get; set; }
    }
}
