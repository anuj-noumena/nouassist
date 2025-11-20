using Microsoft.Extensions.DependencyInjection;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Aggregates;
using NouAssist.RequirementsManagement.CollaborationAccessControl.EntityFrameworkCore.Repositories;
using NouAssist.RequirementsManagement.CollaborationAccessControl.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSQL;
using Volo.Abp.Modularity;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.EntityFrameworkCore
{
    [DependsOn(
        typeof(CollaborationDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSQLModule)
    )]
    public class CollaborationEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // Register the DbContext for Dependency Injection
            context.Services.AddAbpDbContext<CollaborationDbContext>(options =>
            {
                // Register custom repositories
                options.AddRepository<UserAccessProfile, EfCoreUserAccessProfileRepository>();
                options.AddRepository<NotificationConfiguration, EfCoreNotificationConfigurationRepository>();
                options.AddRepository<DiscussionBoard, EfCoreDiscussionBoardRepository>();
            });

            // Configure EF Core to use PostgreSQL
            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }
    }
}
