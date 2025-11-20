using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.ProjectExecution.Reports.Dtos
{
    /// <summary>
    /// DTO for user utilization data
    /// </summary>
    public class UserUtilizationDto : EntityDto
    {
        /// <summary>
        /// ID of the user
        /// </summary>
        public Guid UserId { get; set; }
        
        /// <summary>
        /// Name of the user
        /// </summary>
        public string UserName { get; set; }
        
        /// <summary>
        /// Total hours logged by the user
        /// </summary>
        public double TotalHours { get; set; }
        
        /// <summary>
        /// Standard capacity for the period (hours)
        /// </summary>
        public double Capacity { get; set; }
        
        /// <summary>
        /// Utilization percentage
        /// </summary>
        public double UtilizationPercentage { get; set; }
        
        /// <summary>
        /// Hours broken down by project
        /// </summary>
        public List<ProjectUtilizationDto> ProjectBreakdown { get; set; }
    }
}
