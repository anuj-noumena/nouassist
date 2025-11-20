using System;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Events.Base;
using Volo.Abp.EventBus;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.Events.InventoryItem
{
    /// <summary>
    /// Event raised when a new inventory item is defined
    /// </summary>
    [EventName("NouAssist.BusinessOperations.ResourceInventoryManagement.InventoryItemDefined")]
    public class InventoryItemDefinedEto : RIMEtoBase
    {
        /// <summary>
        /// ID of the inventory item
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Stock keeping unit (SKU) of the inventory item
        /// </summary>
        public string Sku { get; set; }

        /// <summary>
        /// Name of the inventory item
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Category path of the inventory item
        /// </summary>
        public string CategoryPath { get; set; }
    }
}
