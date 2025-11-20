using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.StorageLocation.Dtos
{
    /// <summary>
    /// DTO for getting a list of storage locations with filtering and sorting
    /// </summary>
    public class GetStorageLocationListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// Text to filter locations by code, name, or description
        /// </summary>
        public string? Filter { get; set; }
        
        /// <summary>
        /// Filter by location type
        /// </summary>
        public string? LocationType { get; set; }
        
        /// <summary>
        /// Whether to include only active locations
        /// </summary>
        public bool? IsActive { get; set; }
        
        /// <summary>
        /// Filter by tags (comma-separated)
        /// </summary>
        public string? Tags { get; set; }
        
        /// <summary>
        /// Whether to include child locations
        /// </summary>
        public bool IncludeChildren { get; set; }
    }
}
