using NouAssist.BusinessOperations.ResourceInventoryManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.ResourceType.Dtos
{
    /// <summary>
    /// DTO for getting a list of resource types with filtering and sorting
    /// </summary>
    public class GetResourceTypeListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// Text to filter resource types by name, code, or description
        /// </summary>
        public string? Filter { get; set; }
        
        /// <summary>
        /// Filter by resource category
        /// </summary>
        public ResourceCategory? Category { get; set; }
        
        /// <summary>
        /// Filter by category path
        /// </summary>
        public string? CategoryPath { get; set; }
        
        /// <summary>
        /// Whether to include only active resource types
        /// </summary>
        public bool? IsActive { get; set; }
        
        /// <summary>
        /// Whether to include resource type attributes in results
        /// </summary>
        public bool IncludeAttributes { get; set; }
    }
}
