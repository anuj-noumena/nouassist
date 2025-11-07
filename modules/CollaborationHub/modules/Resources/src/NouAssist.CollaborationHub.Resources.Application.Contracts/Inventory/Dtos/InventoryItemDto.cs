using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.CollaborationHub.Resources.Inventory.Dtos
{
    public class InventoryItemDto : FullAuditedEntityDto<Guid>
    {
        public Guid OwnerId { get; set; }
        public string Kind { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string Barcode { get; set; }
        public string SerialNumber { get; set; }
        public string Sku { get; set; }
        public int Quantity { get; set; }
        public int QuantityAvailable { get; set; }
        public int QuantityCheckedOut { get; set; }
        public string Condition { get; set; }
        public string Status { get; set; }
        public string Location { get; set; }
        public string DisplayLocation { get; set; }
        public string Building { get; set; }
        public string Zone { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? CurrentValue { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? WarrantyExpiresAt { get; set; }
        public string ImageUrl { get; set; }
        public List<string> AdditionalImageUrls { get; set; }
        public Guid? AssignedToUserId { get; set; }
        public DateTime? AssignedAt { get; set; }
        public DateTime? DueBackAt { get; set; }
        public DateTime? LastMaintenanceAt { get; set; }
        public DateTime? NextMaintenanceAt { get; set; }
        public string MaintenanceNotes { get; set; }
        public List<string> Tags { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsOverdue { get; set; }
        public bool NeedsMaintenance { get; set; }
    }
}
