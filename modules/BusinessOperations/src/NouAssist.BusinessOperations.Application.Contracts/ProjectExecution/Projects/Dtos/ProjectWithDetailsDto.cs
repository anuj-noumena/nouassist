using System;
using System.Collections.Generic;

namespace NouAssist.BusinessOperations.ProjectExecution.Projects.Dtos
{
    /// <summary>
    /// DTO for Project entity with additional details
    /// </summary>
    public class ProjectWithDetailsDto : ProjectDto
    {
        /// <summary>
        /// Project phases
        /// </summary>
        public List<ProjectPhaseDto> Phases { get; set; }
        
        /// <summary>
        /// Project milestones
        /// </summary>
        public List<ProjectMilestoneDto> Milestones { get; set; }
        
        /// <summary>
        /// Project members
        /// </summary>
        public List<ProjectMemberDto> Members { get; set; }
        
        /// <summary>
        /// Creates a new instance of ProjectWithDetailsDto
        /// </summary>
        public ProjectWithDetailsDto()
        {
            Phases = new List<ProjectPhaseDto>();
            Milestones = new List<ProjectMilestoneDto>();
            Members = new List<ProjectMemberDto>();
        }
    }
}
