using System;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Enums;
using Volo.Abp.Domain.Entities;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.InventoryItem
{
    /// <summary>
    /// Represents a transaction that affects inventory stock levels
    /// </summary>
    public class InventoryTransaction : Entity<Guid>
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
        /// Type of transaction
        /// </summary>
        public InventoryTransactionType TransactionType { get; private set; }

        /// <summary>
        /// Quantity before the transaction
        /// </summary>
        public decimal PreviousQuantity { get; private set; }

        /// <summary>
        /// Quantity after the transaction
        /// </summary>
        public decimal NewQuantity { get; private set; }

        /// <summary>
        /// Adjustment amount (can be positive or negative)
        /// </summary>
        public decimal AdjustmentAmount { get; private set; }

        /// <summary>
        /// Unit of measure
        /// </summary>
        public string UnitOfMeasure { get; private set; }

        /// <summary>
        /// Reference to related transaction or document
        /// </summary>
        public string TransactionReference { get; private set; }

        /// <summary>
        /// Transaction notes
        /// </summary>
        public string Notes { get; private set; }

        /// <summary>
        /// ID of the operator who performed the transaction
        /// </summary>
        public Guid? OperatorId { get; private set; }

        /// <summary>
        /// Transaction timestamp
        /// </summary>
        public DateTime Timestamp { get; private set; }

        /// <summary>
        /// ID of related transaction (for transfers)
        /// </summary>
        public Guid? RelatedTransactionId { get; private set; }

        protected InventoryTransaction()
        {
        }

        /// <summary>
        /// Creates a new inventory transaction
        /// </summary>
        /// <param name="id">Transaction ID</param>
        /// <param name="inventoryItemId">Inventory item ID</param>
        /// <param name="locationId">Location ID</param>
        /// <param name="transactionType">Transaction type</param>
        /// <param name="previousQuantity">Previous quantity</param>
        /// <param name="newQuantity">New quantity</param>
        /// <param name="adjustmentAmount">Adjustment amount</param>
        /// <param name="unitOfMeasure">Unit of measure</param>
        /// <param name="transactionReference">Transaction reference (optional)</param>
        /// <param name="notes">Transaction notes (optional)</param>
        /// <param name="operatorId">Operator ID (optional)</param>
        public InventoryTransaction(
            Guid id,
            Guid inventoryItemId,
            Guid locationId,
            InventoryTransactionType transactionType,
            decimal previousQuantity,
            decimal newQuantity,
            decimal adjustmentAmount,
            string unitOfMeasure,
            string transactionReference = null,
            string notes = null,
            Guid? operatorId = null) : base(id)
        {
            InventoryItemId = inventoryItemId;
            LocationId = locationId;
            TransactionType = transactionType;
            PreviousQuantity = previousQuantity;
            NewQuantity = newQuantity;
            AdjustmentAmount = adjustmentAmount;
            UnitOfMeasure = unitOfMeasure;
            TransactionReference = transactionReference;
            Notes = notes;
            OperatorId = operatorId;
            Timestamp = DateTime.Now;
        }

        /// <summary>
        /// Sets the related transaction ID for transfers
        /// </summary>
        /// <param name="relatedTransactionId">Related transaction ID</param>
        public void SetRelatedTransactionId(Guid relatedTransactionId)
        {
            if (TransactionType != InventoryTransactionType.Transfer)
            {
                throw new ArgumentException("Related transaction ID can only be set for transfer transactions");
            }

            RelatedTransactionId = relatedTransactionId;
        }
    }
}
