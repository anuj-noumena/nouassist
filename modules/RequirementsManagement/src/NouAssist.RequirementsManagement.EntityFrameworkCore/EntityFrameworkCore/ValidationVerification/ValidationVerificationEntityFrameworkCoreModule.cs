using Microsoft.Extensions.DependencyInjection;
using NouAssist.RequirementsManagement.ValidationVerification.Aggregates;
using NouAssist.RequirementsManagement.ValidationVerification.Repositories;
using NouAssist.RequirementsManagement.ValidationVerification.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSQL;
using Volo.Abp.Modularity;

namespace NouAssist.RequirementsManagement.ValidationVerification.EntityFrameworkCore
{
    [DependsOn(
        typeof(ValidationVerificationDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSQLModule)
    )]
    public class ValidationVerificationEntityFrameworkCoreModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            // Pre-configure any EF Core options if needed
            ValidationVerificationEfCoreEntityExtensionMappings.Configure();
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // Register the DbContext for Dependency Injection
            context.Services.AddAbpDbContext<ValidationVerificationDbContext>(options =>
            {
                // Register CUSTOM repositories here. 
                // Default repositories are registered automatically for Aggregate Roots.
                options.AddRepository<RequirementReviewCycle, EfCoreRequirementReviewCycleRepository>(); 
                options.AddRepository<TestCase, EfCoreTestCaseRepository>(); 

                // Add default repositories for entities (if needed for direct access)
                options.AddDefaultRepositories(includeAllEntities: false);
            });

            // Configure EF Core options
            Configure<AbpDbContextOptions>(options =>
            {
                // Configure to use PostgreSQL
                options.UseNpgsql();
            });
        }
    }

    /// <summary>
    /// Extension mappings for entities - for ABP Suite/Object Extension Manager integration
    /// </summary>
    public static class ValidationVerificationEfCoreEntityExtensionMappings
    {
        private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

        public static void Configure()
        {
            OneTimeRunner.Run(() =>
            {
                /* Configure extension properties for entities here.
                 * This method is called from ValidationVerificationEntityFrameworkCoreModule.
                 * See the documentation for more information: 
                 * https://docs.abp.io/en/abp/latest/Object-Extensions
                 */

                // Example:
                // ObjectExtensionManager.Instance
                //     .MapEfCoreProperty<RequirementReviewCycle, string>(
                //         "CustomProperty",
                //         (entityBuilder, propertyBuilder) =>
                //         {
                //             propertyBuilder.HasMaxLength(128);
                //         }
                //     );

                // Configure any ABP Suite generated extensions or manual extensions here
            });
        }
    }
}
