using System;
using System.Collections.Generic;
using NouAssist.AssetManagement.Resources.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.AssetManagement.Resources.Aggregates
{
    public class InventoryItem : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public const int MaxNameLength = 200;

        public Guid OwnerId { get; private set; }
        public InventoryItemKind Kind { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Manufacturer { get; private set; }
        public string Model { get; private set; }
        public string Barcode { get; private set; }
        public string SerialNumber { get; private set; }
        public string Sku { get; private set; }
        public string Isbn { get; private set; }
        public int Quantity { get; private set; }
        public int QuantityAvailable { get; private set; }
        public InventoryCondition Condition { get; private set; }
        public InventoryStatus Status { get; private set; }
        public string Location { get; private set; }
        public string Building { get; private set; }
        public string Zone { get; private set; }
        public decimal? PurchasePrice { get; private set; }
        public decimal? CurrentValue { get; private set; }
        public DateTime? PurchaseDate { get; private set; }
        public DateTime? WarrantyExpiresAt { get; private set; }
        public string ImageUrl { get; private set; }
        public List<string> AdditionalImageUrls { get; private set; }
        public Guid? AssignedToUserId { get; private set; }
        public DateTime? AssignedAt { get; private set; }
        public DateTime? DueBackAt { get; private set; }
        public DateTime? LastMaintenanceAt { get; private set; }
        public DateTime? NextMaintenanceAt { get; private set; }
        public string MaintenanceNotes { get; private set; }
        public Dictionary<string, object> CustomFields { get; private set; }
        public List<string> Tags { get; private set; }
        public Guid? TenantId { get; private set; }

        protected InventoryItem() { /* ORM */ }

        public InventoryItem(
            Guid id,
            Guid ownerId,
            InventoryItemKind kind,
            string name,
            Guid? tenantId,
            string description = null,
            int quantity = 1,
            string location = null) : base(id)
        {
            OwnerId = Check.NotNull(ownerId, nameof(ownerId));
            Kind = kind;
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), MaxNameLength);
            TenantId = tenantId;
            Description = description;
            Quantity = Check.Positive(quantity, nameof(quantity));
            QuantityAvailable = quantity;
            Location = location;
            
            Condition = InventoryCondition.Good;
            Status = InventoryStatus.Available;
            
            AdditionalImageUrls = new List<string>();
            CustomFields = new Dictionary<string, object>();
            Tags = new List<string>();
        }

        public void CheckOut(Guid userId, int quantityToCheckOut = 1, DateTime? dueDate = null)
        {
            if (quantityToCheckOut <= 0)
            {
                throw new BusinessException("Inventory:InvalidQuantity", "Quantity to check out must be positive.");
            }

            if (QuantityAvailable < quantityToCheckOut)
            {
                throw new BusinessException("Inventory:InsufficientQuantity", "Insufficient quantity available.");
            }

            QuantityAvailable -= quantityToCheckOut;
            
            if (QuantityAvailable == 0)
            {
                Status = InventoryStatus.CheckedOut;
            }

            AssignedToUserId = userId;
            AssignedAt = DateTime.UtcNow;
            DueBackAt = dueDate;
        }

        public void CheckIn(int quantityToCheckIn = 1)
        {
            if (quantityToCheckIn <= 0)
            {
                throw new BusinessException("Inventory:InvalidQuantity", "Quantity to check in must be positive.");
            }

            var newAvailable = Math.Min(QuantityAvailable + quantityToCheckIn, Quantity);
            QuantityAvailable = newAvailable;

            if (QuantityAvailable > 0)
            {
                Status = InventoryStatus.Available;
            }

            if (QuantityAvailable == Quantity)
            {
                AssignedToUserId = null;
                AssignedAt = null;
                DueBackAt = null;
            }
        }

        public void UpdateLocation(string newLocation, string building = null, string zone = null)
        {
            Location = newLocation;
            Building = building ?? Building;
            Zone = zone ?? Zone;
        }

        public void UpdateCondition(InventoryCondition newCondition)
        {
            Condition = newCondition;
        }

        public void ScheduleMaintenance(DateTime maintenanceDate)
        {
            NextMaintenanceAt = maintenanceDate;
            Status = InventoryStatus.Maintenance;
        }

        public void CompleteMaintenance(string notes = null)
        {
            LastMaintenanceAt = DateTime.UtcNow;
            NextMaintenanceAt = null;
            MaintenanceNotes = notes;
            Status = InventoryStatus.Available;
        }

        public void Retire()
        {
            Status = InventoryStatus.Retired;
            QuantityAvailable = 0;
        }

        public void SetPurchaseInfo(decimal? price, DateTime? date)
        {
            PurchasePrice = price;
            PurchaseDate = date;
        }

        public void SetCurrentValue(decimal value)
        {
            CurrentValue = value;
        }

        public void SetWarranty(DateTime expiresAt)
        {
            WarrantyExpiresAt = expiresAt;
        }

        public void AddTag(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag)) return;
            if (Tags.Contains(tag)) return;

            Tags.Add(tag);
        }

        public void RemoveTag(string tag)
        {
            Tags.Remove(tag);
        }

        // Computed properties
        public bool IsAvailable => Status == InventoryStatus.Available && QuantityAvailable > 0;
        public bool IsAssigned => AssignedToUserId.HasValue;
        public bool IsCheckedOut => Status == InventoryStatus.CheckedOut;
        public bool IsUnderMaintenance => Status == InventoryStatus.Maintenance;
        public bool IsRetired => Status == InventoryStatus.Retired;
        public bool HasLocation => !string.IsNullOrWhiteSpace(Location);
        public bool HasBarcode => !string.IsNullOrWhiteSpace(Barcode);
        public bool IsOverdue => DueBackAt.HasValue && DateTime.UtcNow > DueBackAt.Value;
        public bool NeedsMaintenance => NextMaintenanceAt.HasValue && DateTime.UtcNow > NextMaintenanceAt.Value;
        public bool HasWarranty => WarrantyExpiresAt.HasValue && DateTime.UtcNow < WarrantyExpiresAt.Value;
        public bool HasMultipleUnits => Quantity > 1;
        public bool IsFullyAvailable => QuantityAvailable == Quantity;
        public bool IsPartiallyAvailable => QuantityAvailable > 0 && QuantityAvailable < Quantity;
        public bool IsFullyCheckedOut => QuantityAvailable == 0;
        public int QuantityCheckedOut => Quantity - QuantityAvailable;

        public string GetDisplayLocation()
        {
            var parts = new List<string>();
            if (!string.IsNullOrWhiteSpace(Building)) parts.Add(Building);
            if (!string.IsNullOrWhiteSpace(Zone)) parts.Add(Zone);
            if (!string.IsNullOrWhiteSpace(Location)) parts.Add(Location);
            return parts.Count == 0 ? "No location" : string.Join(" > ", parts);
        }

        public void Delete(string deletedBy)
        {
            IsDeleted = true;
            DeleterId = Guid.TryParse(deletedBy, out var id) ? id : null;
            DeletionTime = DateTime.UtcNow;
        }
    }
}
