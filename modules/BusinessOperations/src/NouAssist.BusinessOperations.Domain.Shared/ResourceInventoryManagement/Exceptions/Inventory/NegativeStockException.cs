using System;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Exceptions;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Exceptions.Inventory
{
    /// <summary>
    /// Exception thrown when an inventory transaction would result in negative stock
    /// </summary>
    public class NegativeStockException : RIMDomainException
    {
        /// <summary>
        /// ID of the inventory item
        /// </summary>
        public Guid InventoryItemId { get; }

        /// <summary>
        /// ID of the location
        /// </summary>
        public Guid LocationId { get; }

        /// <summary>
        /// Current stock level
        /// </summary>
        public decimal CurrentStock { get; }

        /// <summary>
        /// Requested reduction amount
        /// </summary>
        public decimal RequestedReduction { get; }

        /// <summary>
        /// Creates a new instance of NegativeStockException
        /// </summary>
        /// <param name="inventoryItemId">ID of the inventory item</param>
        /// <param name="locationId">ID of the location</param>
        /// <param name="currentStock">Current stock level</param>
        /// <param name="requestedReduction">Requested reduction amount</param>
        public NegativeStockException(Guid inventoryItemId, Guid locationId, decimal currentStock, decimal requestedReduction)
            : base("RIM:ErrorMessage:NegativeStockNotAllowed")
        {
            InventoryItemId = inventoryItemId;
            LocationId = locationId;
            CurrentStock = currentStock;
            RequestedReduction = requestedReduction;
            
            WithData("inventoryItemId", inventoryItemId);
            WithData("locationId", locationId);
            WithData("currentStock", currentStock);
            WithData("requestedReduction", requestedReduction);
        }
    }
}
