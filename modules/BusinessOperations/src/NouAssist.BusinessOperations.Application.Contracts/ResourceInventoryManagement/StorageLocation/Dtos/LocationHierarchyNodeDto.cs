using System;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.StorageLocation.Dtos
{
    /// <summary>
    /// DTO for location hierarchy node
    /// </summary>
    public class LocationHierarchyNodeDto
    {
        /// <summary>
        /// ID of the parent location
        /// </summary>
        public Guid ParentLocationId { get; set; }

        /// <summary>
        /// ID of the child location
        /// </summary>
        public Guid ChildLocationId { get; set; }

        /// <summary>
        /// Code of the child location (for display purposes)
        /// </summary>
        public string ChildLocationCode { get; set; } = null!;

        /// <summary>
        /// Name of the child location (for display purposes)
        /// </summary>
        public string ChildLocationName { get; set; } = null!;

        /// <summary>
        /// Display order for UI presentation
        /// </summary>
        public int DisplayOrder { get; set; }
    }
}
