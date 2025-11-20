using System;
using System.Collections.Generic;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Reports.Dtos
{
    /// <summary>
    /// DTO for resource utilization report
    /// </summary>
    public class ResourceUtilizationReportDto
    {
        /// <summary>
        /// Start date of the report period
        /// </summary>
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// End date of the report period
        /// </summary>
        public DateTime EndDate { get; set; }
        
        /// <summary>
        /// Resource type ID filter (if any)
        /// </summary>
        public Guid? ResourceTypeId { get; set; }
        
        /// <summary>
        /// Resource type name (if filtered by type)
        /// </summary>
        public string? ResourceTypeName { get; set; }
        
        /// <summary>
        /// Total number of resources in the report
        /// </summary>
        public int TotalResourceCount { get; set; }
        
        /// <summary>
        /// Average utilization percentage across all resources
        /// </summary>
        public decimal AverageUtilization { get; set; }
        
        /// <summary>
        /// List of resource utilization details
        /// </summary>
        public List<ResourceUtilizationDetailDto> Resources { get; set; } = new();
    }
}
