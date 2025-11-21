using System;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.BusinessOperations.ProjectExecution.Projects.Dtos
{
    /// <summary>
    /// DTO for creating a new project phase
    /// </summary>
    public class CreateProjectPhaseDto
    {
        /// <summary>
        /// Name of the phase
        /// </summary>
        [Required]
        [StringLength(ProjectExecutionConsts.MaxProjectPhaseNameLength)]
        public string Name { get; set; }
        
        /// <summary>
        /// Start date of the phase
        /// </summary>
        [Required]
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// End date of the phase
        /// </summary>
        [Required]
        public DateTime EndDate { get; set; }
        
        /// <summary>
        /// Order of the phase within the project
        /// </summary>
        [Required]
        public int Order { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.BusinessOperations.ProjectExecution.Constants;

namespace NouAssist.BusinessOperations.ProjectExecution.Projects.Dtos
{
    /// <summary>
    /// DTO for creating a new project phase
    /// </summary>
    public class CreateProjectPhaseDto
    {
        /// <summary>
        /// Name of the phase
        /// </summary>
        [Required]
        [StringLength(ProjectExecutionConsts.MaxProjectPhaseNameLength)]
        public string Name { get; set; }
        
        /// <summary>
        /// Start date of the phase
        /// </summary>
        [Required]
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// End date of the phase
        /// </summary>
        [Required]
        public DateTime EndDate { get; set; }
        
        /// <summary>
        /// Order of the phase within the project
        /// </summary>
        [Required]
        [System.ComponentModel.DataAnnotations.Range(1, int.MaxValue)]
        public int Order { get; set; }
    }
}
