using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.ProjectExecution.Projects.Dtos
{
    /// <summary>
    /// DTO for ProjectPhase entity
    /// </summary>
    public class ProjectPhaseDto : EntityDto
    {
        /// <summary>
        /// ID of the project this phase belongs to
        /// </summary>
        public Guid ProjectId { get; set; }
        
        /// <summary>
        /// Name of the phase
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Start date of the phase
        /// </summary>
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// End date of the phase
        /// </summary>
        public DateTime EndDate { get; set; }
        
        /// <summary>
        /// Order of the phase within the project
        /// </summary>
        public int Order { get; set; }
        
        /// <summary>
        /// Completion percentage of the phase
        /// </summary>
        public double CompletionPercentage { get; set; }
    }
}
using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.ProjectExecution.Projects.Dtos
{
    /// <summary>
    /// DTO for ProjectPhase entity
    /// </summary>
    public class ProjectPhaseDto : EntityDto
    {
        /// <summary>
        /// ID of the project this phase belongs to
        /// </summary>
        public Guid ProjectId { get; set; }
        
        /// <summary>
        /// Name of the phase
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Start date of the phase
        /// </summary>
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// End date of the phase
        /// </summary>
        public DateTime EndDate { get; set; }
        
        /// <summary>
        /// Order of the phase within the project
        /// </summary>
        public int Order { get; set; }
        
        /// <summary>
        /// Completion percentage of the phase
        /// </summary>
        public double CompletionPercentage { get; set; }
    }
}
