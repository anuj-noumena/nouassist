using System;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Reports.Dtos
{
    /// <summary>
    /// DTO for resource utilization detail
    /// </summary>
    public class ResourceUtilizationDetailDto
    {
        /// <summary>
        /// Resource instance ID
        /// </summary>
        public Guid ResourceId { get; set; }
        
        /// <summary>
        /// Resource name
        /// </summary>
        public string ResourceName { get; set; } = null!;
        
        /// <summary>
        /// Resource identifier
        /// </summary>
        public string ResourceIdentifier { get; set; } = null!;
        
        /// <summary>
        /// Resource type ID
        /// </summary>
        public Guid ResourceTypeId { get; set; }
        
        /// <summary>
        /// Resource type name
        /// </summary>
        public string ResourceTypeName { get; set; } = null!;
        
        /// <summary>
        /// Utilization percentage (0-100)
        /// </summary>
        public decimal Utilization { get; set; }
        
        /// <summary>
        /// Total duration of bookings in minutes
        /// </summary>
        public int TotalBookedMinutes { get; set; }
        
        /// <summary>
        /// Total number of bookings
        /// </summary>
        public int BookingCount { get; set; }
    }
}
