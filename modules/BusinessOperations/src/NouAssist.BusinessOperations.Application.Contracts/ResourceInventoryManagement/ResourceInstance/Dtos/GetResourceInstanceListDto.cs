using System;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.ResourceInstance.Dtos
{
    /// <summary>
    /// DTO for getting a list of resource instances with filtering and sorting
    /// </summary>
    public class GetResourceInstanceListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// Text to filter resource instances by name, identifier, or description
        /// </summary>
        public string? Filter { get; set; }
        
        /// <summary>
        /// Filter by resource type ID
        /// </summary>
        public Guid? ResourceTypeId { get; set; }
        
        /// <summary>
        /// Filter by resource status
        /// </summary>
        public ResourceStatus? Status { get; set; }
        
        /// <summary>
        /// Filter by associated user ID (for human resources)
        /// </summary>
        public Guid? UserId { get; set; }
        
        /// <summary>
        /// Filter by tags (comma-separated)
        /// </summary>
        public string? Tags { get; set; }
        
        /// <summary>
        /// Whether to include attribute values in results
        /// </summary>
        public bool IncludeAttributes { get; set; }
        
        /// <summary>
        /// Whether to include current allocation in results
        /// </summary>
        public bool IncludeCurrentAllocation { get; set; }
    }
}
