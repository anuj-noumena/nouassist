using Microsoft.Extensions.DependencyInjection;
using NouAssist.Collaboration.Comments.Aggregates;
using NouAssist.Collaboration.EntityFrameworkCore.Comments.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.Collaboration.EntityFrameworkCore;

[DependsOn(
    typeof(CollaborationDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class CollaborationEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<CollaborationDbContext>(options =>
        {
            options.AddDefaultRepositories<ICollaborationDbContext>(includeAllEntities: true);
            options.AddRepository<Comment, EfCoreCommentRepository>();
            options.AddDefaultRepositories(includeAllEntities: true);
        });
    }
}
