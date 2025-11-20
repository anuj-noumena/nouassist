using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.BusinessOperations.ProjectExecution.Projects.Dtos
{
    /// <summary>
    /// DTO for creating a new project milestone
    /// </summary>
    public class CreateProjectMilestoneDto
    {
        /// <summary>
        /// Name of the milestone
        /// </summary>
        [Required]
        [StringLength(ProjectExecutionConsts.MaxProjectMilestoneNameLength)]
        public string Name { get; set; }
        
        /// <summary>
        /// Target date for the milestone
        /// </summary>
        [Required]
        public DateTime TargetDate { get; set; }
        
        /// <summary>
        /// Optional description of the milestone
        /// </summary>
        [StringLength(1000)]
        public string Description { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.BusinessOperations.ProjectExecution.Constants;

namespace NouAssist.BusinessOperations.ProjectExecution.Projects.Dtos
{
    /// <summary>
    /// DTO for creating a new project milestone
    /// </summary>
    public class CreateProjectMilestoneDto
    {
        /// <summary>
        /// Name of the milestone
        /// </summary>
        [Required]
        [StringLength(ProjectExecutionConsts.MaxProjectMilestoneNameLength)]
        public string Name { get; set; }
        
        /// <summary>
        /// Target date for the milestone
        /// </summary>
        [Required]
        public DateTime TargetDate { get; set; }
        
        /// <summary>
        /// Optional description of the milestone
        /// </summary>
        [StringLength(1000)]
        public string Description { get; set; }
    }
}
