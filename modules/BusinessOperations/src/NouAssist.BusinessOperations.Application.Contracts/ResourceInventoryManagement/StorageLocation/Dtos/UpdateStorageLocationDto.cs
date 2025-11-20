using System.ComponentModel.DataAnnotations;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Constants;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.StorageLocation.Dtos
{
    /// <summary>
    /// DTO for updating a storage location
    /// </summary>
    public class UpdateStorageLocationDto
    {
        /// <summary>
        /// Name of the storage location
        /// </summary>
        [Required]
        [StringLength(RIMConstants.StorageLocations.MaxNameLength)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Description of the storage location
        /// </summary>
        [StringLength(RIMConstants.StorageLocations.MaxDescriptionLength)]
        public string? Description { get; set; }

        /// <summary>
        /// Type of location (warehouse, shelf, bin, etc.)
        /// </summary>
        [Required]
        [StringLength(100)]
        public string LocationType { get; set; } = null!;

        /// <summary>
        /// Physical address of the location (optional)
        /// </summary>
        [StringLength(500)]
        public string? Address { get; set; }

        /// <summary>
        /// Geographic coordinates of the location (optional)
        /// </summary>
        public LocationCoordinatesDto? Coordinates { get; set; }

        /// <summary>
        /// Maximum capacity of the location (optional)
        /// </summary>
        [Range(0, double.MaxValue)]
        public decimal? MaxCapacity { get; set; }

        /// <summary>
        /// Unit of measure for the capacity
        /// </summary>
        [StringLength(50)]
        public string? CapacityUnitOfMeasure { get; set; }

        /// <summary>
        /// Additional tags for categorization and searching
        /// </summary>
        [StringLength(500)]
        public string? Tags { get; set; }
    }
}
