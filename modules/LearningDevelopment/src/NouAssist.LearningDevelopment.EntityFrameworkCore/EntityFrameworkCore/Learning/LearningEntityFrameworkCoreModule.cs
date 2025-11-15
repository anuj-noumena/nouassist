using Microsoft.Extensions.DependencyInjection;
using NouAssist.Learning.Aggregates;
using NouAssist.Learning.Data;
using NouAssist.Learning.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.Modularity;

namespace NouAssist.Learning.EntityFrameworkCore
{
    [DependsOn(
        typeof(LearningDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSqlModule)
    )]
    public class LearningEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // Register the DbContext for Dependency Injection
            context.Services.AddAbpDbContext<LearningDbContext>(options =>
            {
                // Register custom repositories
                options.AddRepository<Topic, EfCoreTopicRepository>();
                options.AddRepository<LearningSession, EfCoreLearningSessionRepository>();
                options.AddRepository<LearningNote, EfCoreLearningNoteRepository>();
                options.AddRepository<Mindmap, EfCoreMindmapRepository>();
                options.AddRepository<BookReadingHistory, EfCoreBookReadingHistoryRepository>();

                // Enable default repositories for all aggregate roots
                options.AddDefaultRepositories(includeAllEntities: false);
            });

            // Configure EF Core to use PostgreSQL
            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }
    }
}
