using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.ProjectExecution.Projects.Dtos
{
    /// <summary>
    /// DTO for updating a project
    /// </summary>
    public class UpdateProjectDto
    {
        /// <summary>
        /// Name of the project
        /// </summary>
        [Required]
        [StringLength(ProjectExecutionConsts.MaxProjectNameLength)]
        public string Name { get; set; }
        
        /// <summary>
        /// Description of the project
        /// </summary>
        [Required]
        [StringLength(ProjectExecutionConsts.MaxProjectDescriptionLength)]
        public string Description { get; set; }
        
        /// <summary>
        /// Start date of the project
        /// </summary>
        [Required]
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// Planned end date of the project
        /// </summary>
        [Required]
        public DateTime PlannedEndDate { get; set; }
        
        /// <summary>
        /// Optional ID of the organization associated with the project
        /// </summary>
        public Guid? OrganizationId { get; set; }
        
        /// <summary>
        /// Optional ID of the contact associated with the project
        /// </summary>
        public Guid? ContactId { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.BusinessOperations.ProjectExecution.Constants;

namespace NouAssist.BusinessOperations.ProjectExecution.Projects.Dtos
{
    /// <summary>
    /// DTO for updating an existing project
    /// </summary>
    public class UpdateProjectDto
    {
        /// <summary>
        /// Name of the project
        /// </summary>
        [Required]
        [StringLength(ProjectExecutionConsts.MaxProjectNameLength)]
        public string Name { get; set; }
        
        /// <summary>
        /// Description of the project
        /// </summary>
        [Required]
        [StringLength(ProjectExecutionConsts.MaxProjectDescriptionLength)]
        public string Description { get; set; }
        
        /// <summary>
        /// Start date of the project
        /// </summary>
        [Required]
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// Planned end date of the project
        /// </summary>
        [Required]
        public DateTime PlannedEndDate { get; set; }
        
        /// <summary>
        /// ID of the organization associated with the project
        /// </summary>
        public Guid? OrganizationId { get; set; }
        
        /// <summary>
        /// ID of the contact associated with the project
        /// </summary>
        public Guid? ContactId { get; set; }
    }
}
