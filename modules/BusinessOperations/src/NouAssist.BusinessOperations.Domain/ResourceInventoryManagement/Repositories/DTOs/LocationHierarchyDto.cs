using System;
using System.Collections.Generic;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Repositories
{
    /// <summary>
    /// Data transfer object for representing a location in a hierarchy
    /// </summary>
    public class LocationHierarchyDto
    {
        /// <summary>
        /// ID of the location
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Code of the location
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Name of the location
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Location type
        /// </summary>
        public string LocationType { get; set; }

        /// <summary>
        /// Child locations
        /// </summary>
        public List<LocationHierarchyDto> Children { get; set; } = new List<LocationHierarchyDto>();
    }
}
