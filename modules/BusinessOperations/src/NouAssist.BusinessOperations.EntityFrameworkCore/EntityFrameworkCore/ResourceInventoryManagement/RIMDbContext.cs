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
    /// DbContext implementation for the RIM module
    /// </summary>
    [ConnectionStringName(RIMDbProperties.ConnectionStringName)]
    public class RIMDbContext : AbpDbContext<RIMDbContext>, IRIMDbContext
    {
        // Resource Management
        public DbSet<ResourceTypeAggregate> ResourceTypes { get; set; } = null!;
        public DbSet<ResourceAttributeDefinition> ResourceAttributeDefinitions { get; set; } = null!;
        public DbSet<ResourceInstanceAggregate> ResourceInstances { get; set; } = null!;
        public DbSet<AllocationRecord> AllocationRecords { get; set; } = null!;
        public DbSet<UsageRecord> UsageRecords { get; set; } = null!;

        // Inventory Management
        public DbSet<InventoryItemDefinitionAggregate> InventoryItems { get; set; } = null!;
        public DbSet<StockLevel> StockLevels { get; set; } = null!;
        public DbSet<InventoryTransaction> InventoryTransactions { get; set; } = null!;

        // Resource Booking
        public DbSet<ResourceBookingAggregate> ResourceBookings { get; set; } = null!;

        // Storage Locations
        public DbSet<StorageLocationAggregate> StorageLocations { get; set; } = null!;
        public DbSet<LocationHierarchyNode> LocationHierarchyNodes { get; set; } = null!;

        // Inventory Audit
        public DbSet<InventoryAuditSessionAggregate> InventoryAuditSessions { get; set; } = null!;
        public DbSet<AuditDiscrepancy> AuditDiscrepancies { get; set; } = null!;

        /// <summary>
        /// Creates a new instance of RIMDbContext
        /// </summary>
        /// <param name="options">DbContext options</param>
        public RIMDbContext(DbContextOptions<RIMDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Configures the model
        /// </summary>
        /// <param name="builder">Model builder</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Call base first to apply ABP conventions
            base.OnModelCreating(builder);

            // Apply RIM-specific configurations
            builder.ConfigureRIM(this);
        }
    }
}
