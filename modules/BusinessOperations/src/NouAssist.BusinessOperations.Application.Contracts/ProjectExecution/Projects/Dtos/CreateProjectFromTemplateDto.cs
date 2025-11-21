using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.BusinessOperations.ProjectExecution.Constants;

namespace NouAssist.BusinessOperations.ProjectExecution.Projects.Dtos
{
    /// <summary>
    /// DTO for creating a project from a template
    /// </summary>
    public class CreateProjectFromTemplateDto
    {
        /// <summary>
        /// Name of the template to use
        /// </summary>
        [Required]
        [StringLength(100)]
        public string TemplateName { get; set; }
        
        /// <summary>
        /// Name of the project
        /// </summary>
        [Required]
        [StringLength(ProjectExecutionConsts.MaxProjectNameLength)]
        public string ProjectName { get; set; }
        
        /// <summary>
        /// Description of the project
        /// </summary>
        [Required]
        [StringLength(ProjectExecutionConsts.MaxProjectDescriptionLength)]
        public string ProjectDescription { get; set; }
        
        /// <summary>
        /// Start date of the project
        /// </summary>
        [Required]
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// Planned end date of the project
        /// </summary>
        [Required]
        public DateTime EndDate { get; set; }
    }
}
