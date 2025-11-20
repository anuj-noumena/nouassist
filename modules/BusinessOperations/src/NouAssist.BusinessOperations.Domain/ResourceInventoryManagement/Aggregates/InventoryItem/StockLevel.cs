using System;
using Volo.Abp.Domain.Entities;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.InventoryItem
{
    /// <summary>
    /// Represents the stock level of an inventory item at a specific location
    /// </summary>
    public class StockLevel : Entity
    {
        /// <summary>
        /// ID of the inventory item
        /// </summary>
        public Guid InventoryItemId { get; private set; }

        /// <summary>
        /// ID of the storage location
        /// </summary>
        public Guid LocationId { get; private set; }

        /// <summary>
        /// Quantity on hand at this location
        /// </summary>
        public decimal Quantity { get; private set; }

        /// <summary>
        /// Last update time of this stock level
        /// </summary>
        public DateTime LastUpdated { get; private set; }

        protected StockLevel()
        {
        }

        /// <summary>
        /// Creates a new stock level
        /// </summary>
        /// <param name="inventoryItemId">Inventory item ID</param>
        /// <param name="locationId">Location ID</param>
        /// <param name="quantity">Initial quantity</param>
        public StockLevel(
            Guid inventoryItemId,
            Guid locationId,
            decimal quantity)
        {
            InventoryItemId = inventoryItemId;
            LocationId = locationId;
            Quantity = quantity;
            LastUpdated = DateTime.Now;
        }

        /// <summary>
        /// Updates the quantity
        /// </summary>
        /// <param name="newQuantity">New quantity</param>
        public void UpdateQuantity(decimal newQuantity)
        {
            Quantity = newQuantity;
            LastUpdated = DateTime.Now;
        }

        /// <summary>
        /// Gets the entity's keys for comparison
        /// </summary>
        /// <returns>Array of key values</returns>
        public override object[] GetKeys()
        {
            return new object[] { InventoryItemId, LocationId };
        }
    }
}
