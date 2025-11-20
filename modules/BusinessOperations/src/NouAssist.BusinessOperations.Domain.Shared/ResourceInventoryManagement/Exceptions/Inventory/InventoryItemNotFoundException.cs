using System;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Exceptions;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Exceptions.Inventory
{
    /// <summary>
    /// Exception thrown when an inventory item cannot be found
    /// </summary>
    public class InventoryItemNotFoundException : RIMDomainException
    {
        /// <summary>
        /// ID of the inventory item that was not found
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Creates a new instance of InventoryItemNotFoundException
        /// </summary>
        /// <param name="id">ID of the inventory item that was not found</param>
        public InventoryItemNotFoundException(Guid id)
            : base("RIM:ErrorMessage:InventoryItemNotFound")
        {
            Id = id;
            WithData("id", id);
        }

        /// <summary>
        /// Creates a new instance of InventoryItemNotFoundException for an inventory item SKU
        /// </summary>
        /// <param name="sku">SKU of the inventory item that was not found</param>
        public InventoryItemNotFoundException(string sku)
            : base("RIM:ErrorMessage:InventoryItemNotFoundBySku")
        {
            WithData("sku", sku);
        }
    }
}
