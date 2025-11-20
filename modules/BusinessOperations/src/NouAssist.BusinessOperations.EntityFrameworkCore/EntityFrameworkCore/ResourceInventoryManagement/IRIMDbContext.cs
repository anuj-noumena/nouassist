using Microsoft.EntityFrameworkCore;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.InventoryAudit;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.InventoryItem;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.ResourceBooking;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.ResourceInstance;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.ResourceType;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.StorageLocation;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.EntityFrameworkCore
{
    /// <summary>
    /// DbContext interface for the RIM module
    /// </summary>
    [ConnectionStringName(RIMDbProperties.ConnectionStringName)]
    public interface IRIMDbContext : IEfCoreDbContext
    {
        /// <summary>
        /// Resource types
        /// </summary>
        DbSet<ResourceTypeAggregate> ResourceTypes { get; }

        /// <summary>
        /// Resource attribute definitions
        /// </summary>
        DbSet<ResourceAttributeDefinition> ResourceAttributeDefinitions { get; }

        /// <summary>
        /// Resource instances
        /// </summary>
        DbSet<ResourceInstanceAggregate> ResourceInstances { get; }

        /// <summary>
        /// Resource allocation records
        /// </summary>
        DbSet<AllocationRecord> AllocationRecords { get; }

        /// <summary>
        /// Resource usage records
        /// </summary>
        DbSet<UsageRecord> UsageRecords { get; }

        /// <summary>
        /// Inventory item definitions
        /// </summary>
        DbSet<InventoryItemDefinitionAggregate> InventoryItems { get; }

        /// <summary>
        /// Inventory stock levels
        /// </summary>
        DbSet<StockLevel> StockLevels { get; }

        /// <summary>
        /// Inventory transactions
        /// </summary>
        DbSet<InventoryTransaction> InventoryTransactions { get; }

        /// <summary>
        /// Resource bookings
        /// </summary>
        DbSet<ResourceBookingAggregate> ResourceBookings { get; }

        /// <summary>
        /// Storage locations
        /// </summary>
        DbSet<StorageLocationAggregate> StorageLocations { get; }

        /// <summary>
        /// Location hierarchy nodes
        /// </summary>
        DbSet<LocationHierarchyNode> LocationHierarchyNodes { get; }

        /// <summary>
        /// Inventory audit sessions
        /// </summary>
        DbSet<InventoryAuditSessionAggregate> InventoryAuditSessions { get; }

        /// <summary>
        /// Audit discrepancies
        /// </summary>
        DbSet<AuditDiscrepancy> AuditDiscrepancies { get; }
    }
}
