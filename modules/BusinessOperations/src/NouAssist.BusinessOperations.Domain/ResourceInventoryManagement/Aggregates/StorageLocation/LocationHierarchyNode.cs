using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.StorageLocation
{
    /// <summary>
    /// Represents a parent-child relationship in the location hierarchy
    /// </summary>
    public class LocationHierarchyNode : Entity
    {
        /// <summary>
        /// ID of the parent location
        /// </summary>
        public Guid ParentLocationId { get; private set; }

        /// <summary>
        /// ID of the child location
        /// </summary>
        public Guid ChildLocationId { get; private set; }

        /// <summary>
        /// Code of the child location (for display purposes)
        /// </summary>
        public string ChildLocationCode { get; private set; }

        /// <summary>
        /// Name of the child location (for display purposes)
        /// </summary>
        public string ChildLocationName { get; private set; }

        /// <summary>
        /// Display order for UI presentation
        /// </summary>
        public int DisplayOrder { get; private set; }

        protected LocationHierarchyNode()
        {
        }

        /// <summary>
        /// Creates a new location hierarchy node
        /// </summary>
        /// <param name="parentLocationId">Parent location ID</param>
        /// <param name="childLocationId">Child location ID</param>
        /// <param name="childLocationCode">Child location code</param>
        /// <param name="childLocationName">Child location name</param>
        /// <param name="displayOrder">Display order (optional)</param>
        public LocationHierarchyNode(
            Guid parentLocationId,
            Guid childLocationId,
            string childLocationCode,
            string childLocationName,
            int displayOrder = 0)
        {
            ParentLocationId = parentLocationId;
            ChildLocationId = childLocationId;
            SetChildLocationCode(childLocationCode);
            SetChildLocationName(childLocationName);
            DisplayOrder = displayOrder;
        }

        /// <summary>
        /// Updates the child location display information
        /// </summary>
        /// <param name="childLocationCode">Updated child location code</param>
        /// <param name="childLocationName">Updated child location name</param>
        public void UpdateChildLocationInfo(string childLocationCode, string childLocationName)
        {
            SetChildLocationCode(childLocationCode);
            SetChildLocationName(childLocationName);
        }

        /// <summary>
        /// Updates the display order
        /// </summary>
        /// <param name="displayOrder">New display order</param>
        public void UpdateDisplayOrder(int displayOrder)
        {
            DisplayOrder = displayOrder;
        }

        private void SetChildLocationCode(string childLocationCode)
        {
            ChildLocationCode = Check.NotNullOrWhiteSpace(
                childLocationCode,
                nameof(childLocationCode),
                maxLength: Constants.RIMConstants.StorageLocations.MaxCodeLength
            );
        }

        private void SetChildLocationName(string childLocationName)
        {
            ChildLocationName = Check.NotNullOrWhiteSpace(
                childLocationName,
                nameof(childLocationName),
                maxLength: Constants.RIMConstants.StorageLocations.MaxNameLength
            );
        }

        /// <summary>
        /// Gets the entity's keys for comparison
        /// </summary>
        /// <returns>Array of key values</returns>
        public override object[] GetKeys()
        {
            return new object[] { ParentLocationId, ChildLocationId };
        }
    }
}
