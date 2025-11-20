using Microsoft.Extensions.DependencyInjection;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Aggregates;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Entities;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.EntityFrameworkCore.Repositories;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSQL;
using Volo.Abp.Modularity;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.EntityFrameworkCore
{
    [DependsOn(
        typeof(KnowledgeInnovationDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSQLModule)
    )]
    public class KnowledgeInnovationEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // Register the DbContext
            context.Services.AddAbpDbContext<KnowledgeInnovationDbContext>(options =>
            {
                // Register custom repositories
                options.AddRepository<KnowledgeAsset, EfCoreKnowledgeAssetRepository>();
                options.AddRepository<LearningResource, EfCoreLearningResourceRepository>();
                options.AddRepository<Idea, EfCoreIdeaRepository>();
                options.AddRepository<DecisionRecord, EfCoreDecisionRecordRepository>();
                options.AddRepository<MentorProfile, EfCoreMentorProfileRepository>();
                options.AddRepository<Tag, EfCoreTagRepository>();
            });

            // Configure EF Core options
            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }
    }
}
