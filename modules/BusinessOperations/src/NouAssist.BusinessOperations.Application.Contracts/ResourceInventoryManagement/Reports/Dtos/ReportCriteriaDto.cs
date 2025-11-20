using System;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Reports.Dtos
{
    /// <summary>
    /// DTO for report criteria
    /// </summary>
    public class ReportCriteriaDto
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
        /// Resource type ID filter (for resource reports)
        /// </summary>
        public Guid? ResourceTypeId { get; set; }
        
        /// <summary>
        /// Location ID filter (for inventory reports)
        /// </summary>
        public Guid? LocationId { get; set; }
        
        /// <summary>
        /// Category path filter (for inventory reports)
        /// </summary>
        public string? CategoryPath { get; set; }
    }
}
