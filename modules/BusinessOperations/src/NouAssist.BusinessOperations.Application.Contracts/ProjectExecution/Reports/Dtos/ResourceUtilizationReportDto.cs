using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.ProjectExecution.Reports.Dtos
{
    /// <summary>
    /// DTO for resource utilization report
    /// </summary>
    public class ResourceUtilizationReportDto : EntityDto
    {
        /// <summary>
        /// Start date of the report
        /// </summary>
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// End date of the report
        /// </summary>
        public DateTime EndDate { get; set; }
        
        /// <summary>
        /// Utilization data by user
        /// </summary>
        public List<UserUtilizationDto> UserUtilizations { get; set; }
    }
}
