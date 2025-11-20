using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.StorageLocation.Dtos
{
    /// <summary>
    /// DTO for storage location information
    /// </summary>
    public class StorageLocationDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// Code uniquely identifying this location
        /// </summary>
        public string Code { get; set; } = null!;

        /// <summary>
        /// Name of the storage location
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Description of the storage location
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// ID of the parent location in the hierarchy
        /// </summary>
        public Guid? ParentLocationId { get; set; }
        
        /// <summary>
        /// Name of the parent location
        /// </summary>
        public string? ParentLocationName { get; set; }

        /// <summary>
        /// Type of location (warehouse, shelf, bin, etc.)
        /// </summary>
        public string LocationType { get; set; } = null!;

        /// <summary>
        /// Physical address of the location (optional)
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Geographic coordinates of the location (optional)
        /// </summary>
        public LocationCoordinatesDto? Coordinates { get; set; }

        /// <summary>
        /// Maximum capacity of the location (optional)
        /// </summary>
        public decimal? MaxCapacity { get; set; }

        /// <summary>
        /// Unit of measure for the capacity
        /// </summary>
        public string? CapacityUnitOfMeasure { get; set; }

        /// <summary>
        /// Indicates if this storage location is active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Additional tags for categorization and searching
        /// </summary>
        public string? Tags { get; set; }
        
        /// <summary>
        /// List of child locations in the hierarchy
        /// </summary>
        public List<StorageLocationDto>? ChildLocations { get; set; }
    }
}
