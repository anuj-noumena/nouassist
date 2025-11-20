using System;
using System.Collections.Generic;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.StorageLocation.Dtos
{
    /// <summary>
    /// DTO for location tree node (hierarchical view)
    /// </summary>
    public class LocationTreeNodeDto
    {
        /// <summary>
        /// Location ID
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Location code
        /// </summary>
        public string Code { get; set; } = null!;
        
        /// <summary>
        /// Location name
        /// </summary>
        public string Name { get; set; } = null!;
        
        /// <summary>
        /// Location type
        /// </summary>
        public string LocationType { get; set; } = null!;
        
        /// <summary>
        /// Child locations
        /// </summary>
        public List<LocationTreeNodeDto> Children { get; set; } = new();
    }
}
