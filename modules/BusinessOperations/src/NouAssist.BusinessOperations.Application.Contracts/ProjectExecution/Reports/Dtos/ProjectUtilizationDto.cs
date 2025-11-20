using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.ProjectExecution.Reports.Dtos
{
    /// <summary>
    /// DTO for project utilization data
    /// </summary>
    public class ProjectUtilizationDto : EntityDto
    {
        /// <summary>
        /// ID of the project
        /// </summary>
        public Guid ProjectId { get; set; }
        
        /// <summary>
        /// Name of the project
        /// </summary>
        public string ProjectName { get; set; }
        
        /// <summary>
        /// Hours logged on this project
        /// </summary>
        public double Hours { get; set; }
        
        /// <summary>
        /// Percentage of total hours
        /// </summary>
        public double Percentage { get; set; }
    }
}
