using Microsoft.Extensions.DependencyInjection;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.InventoryAudit;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.InventoryItem;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.ResourceBooking;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.ResourceInstance;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.ResourceType;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Aggregates.StorageLocation;
using NouAssist.BusinessOperations.ResourceInventoryManagement.EntityFrameworkCore.Repositories;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSQL;
using Volo.Abp.Modularity;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.EntityFrameworkCore
{
    /// <summary>
    /// EntityFrameworkCore module for the RIM module
    /// </summary>
    [DependsOn(
        typeof(RIMDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSQLModule)
    )]
    public class RIMEntityFrameworkCoreModule : AbpModule
    {
        /// <summary>
        /// Pre-configures services for the module
        /// </summary>
        /// <param name="context">Pre-configuration context</param>
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            // Optional: Add any pre-configurations here
        }

        /// <summary>
        /// Configures services for the module
        /// </summary>
        /// <param name="context">Service configuration context</param>
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // Configure EF Core DbContext and repositories
            context.Services.AddAbpDbContext<RIMDbContext>(options =>
            {
                // Add repository registrations for all aggregates
                options.AddRepository<ResourceTypeAggregate, EfCoreResourceTypeRepository>();
                options.AddRepository<ResourceInstanceAggregate, EfCoreResourceInstanceRepository>();
                options.AddRepository<InventoryItemDefinitionAggregate, EfCoreInventoryItemRepository>();
                options.AddRepository<ResourceBookingAggregate, EfCoreResourceBookingRepository>();
                options.AddRepository<StorageLocationAggregate, EfCoreStorageLocationRepository>();
                options.AddRepository<InventoryAuditSessionAggregate, EfCoreInventoryAuditRepository>();
            });

            // Configure EF Core DB provider
            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }

        /// <summary>
        /// Post-configures services for the module
        /// </summary>
        /// <param name="context">Post-configuration context</param>
        public override void PostConfigureServices(ServiceConfigurationContext context)
        {
            // Optional: Add any post-configurations here
        }
    }
}
