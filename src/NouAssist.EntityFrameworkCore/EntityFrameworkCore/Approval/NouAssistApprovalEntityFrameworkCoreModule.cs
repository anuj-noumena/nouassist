using Microsoft.Extensions.DependencyInjection;
using NouAssist.Approval.Aggregates;
using NouAssist.Approval.Data;
using NouAssist.Approval.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSQL;
using Volo.Abp.Modularity;

namespace NouAssist.Approval.EntityFrameworkCore
{
    [DependsOn(
        typeof(NouAssistApprovalDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSQLModule)
    )]
    public class NouAssistApprovalEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<ApprovalDbContext>(options =>
            {
                options.AddRepository<ApprovalWorkflow, EfCoreApprovalWorkflowRepository>();
                options.AddRepository<ApprovalRequest, EfCoreApprovalRequestRepository>();

                options.AddDefaultRepositories(includeAllEntities: true);
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }
    }
}
