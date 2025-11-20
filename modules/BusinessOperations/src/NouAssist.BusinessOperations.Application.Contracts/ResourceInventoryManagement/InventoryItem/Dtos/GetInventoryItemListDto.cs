using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.InventoryItem.Dtos
{
    /// <summary>
    /// DTO for getting a list of inventory items with filtering and sorting
    /// </summary>
    public class GetInventoryItemListDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// Text to filter inventory items by name, SKU, description, or barcode
        /// </summary>
        public string? Filter { get; set; }
        
        /// <summary>
        /// Filter by category path
        /// </summary>
        public string? CategoryPath { get; set; }
        
        /// <summary>
        /// Whether to include child categories in category path filter
        /// </summary>
        public bool IncludeChildCategories { get; set; }
        
        /// <summary>
        /// Whether to include only active inventory items
        /// </summary>
        public bool? IsActive { get; set; }
        
        /// <summary>
        /// Filter by tags (comma-separated)
        /// </summary>
        public string? Tags { get; set; }
        
        /// <summary>
        /// Whether to include stock levels in results
        /// </summary>
        public bool IncludeStockLevels { get; set; }
        
        /// <summary>
        /// Filter by items below minimum stock level
        /// </summary>
        public bool? BelowMinimumStockLevel { get; set; }
    }
}
