using System;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Events.InventoryItem
{
    /// <summary>
    /// Event raised when a stock level is adjusted
    /// </summary>
    [EventName("NouAssist.BusinessOperations.ResourceInventoryManagement.StockLevelAdjusted")]
    public class StockLevelAdjustedEto : RIMEtoBase
    {
        /// <summary>
        /// ID of the inventory item
        /// </summary>
        public Guid InventoryItemId { get; set; }

        /// <summary>
        /// ID of the storage location
        /// </summary>
        public Guid LocationId { get; set; }

        /// <summary>
        /// Previous quantity
        /// </summary>
        public decimal PreviousQuantity { get; set; }

        /// <summary>
        /// New quantity
        /// </summary>
        public decimal NewQuantity { get; set; }

        /// <summary>
        /// Adjustment amount (can be positive or negative)
        /// </summary>
        public decimal AdjustmentAmount { get; set; }

        /// <summary>
        /// Unit of measure
        /// </summary>
        public string UnitOfMeasure { get; set; }

        /// <summary>
        /// Type of transaction that caused this adjustment
        /// </summary>
        public string TransactionType { get; set; }

        /// <summary>
        /// Reference to related transaction or document
        /// </summary>
        public string TransactionReference { get; set; }
    }
}
