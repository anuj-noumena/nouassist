using System;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.InventoryItem.Dtos
{
    /// <summary>
    /// DTO for inventory transaction information
    /// </summary>
    public class InventoryTransactionDto : EntityDto<Guid>
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
        /// Name of the storage location
        /// </summary>
        public string LocationName { get; set; } = null!;

        /// <summary>
        /// Type of transaction
        /// </summary>
        public InventoryTransactionType TransactionType { get; set; }

        /// <summary>
        /// Quantity before the transaction
        /// </summary>
        public decimal PreviousQuantity { get; set; }

        /// <summary>
        /// Quantity after the transaction
        /// </summary>
        public decimal NewQuantity { get; set; }

        /// <summary>
        /// Adjustment amount (can be positive or negative)
        /// </summary>
        public decimal AdjustmentAmount { get; set; }

        /// <summary>
        /// Unit of measure
        /// </summary>
        public string UnitOfMeasure { get; set; } = null!;

        /// <summary>
        /// Reference to related transaction or document
        /// </summary>
        public string? TransactionReference { get; set; }

        /// <summary>
        /// Transaction notes
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// ID of the operator who performed the transaction
        /// </summary>
        public Guid? OperatorId { get; set; }

        /// <summary>
        /// Transaction timestamp
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// ID of related transaction (for transfers)
        /// </summary>
        public Guid? RelatedTransactionId { get; set; }
    }
}
