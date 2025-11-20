using Microsoft.Extensions.DependencyInjection;
using NouAssist.RequirementsManagement.Elicitation.Aggregates;
using NouAssist.RequirementsManagement.Elicitation.Entities;
using NouAssist.RequirementsManagement.Elicitation.EntityFrameworkCore.Repositories;
using NouAssist.RequirementsManagement.Elicitation.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSQL;
using Volo.Abp.Modularity;

namespace NouAssist.RequirementsManagement.Elicitation.EntityFrameworkCore
{
    [DependsOn(
        typeof(ElicitationDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSQLModule)
    )]
    public class ElicitationEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // Register the DbContext for Dependency Injection
            context.Services.AddAbpDbContext<ElicitationDbContext>(options =>
            {
                // Register custom repositories
                options.AddRepository<StakeholderProfile, EfCoreStakeholderProfileRepository>();
                options.AddRepository<ElicitationSession, EfCoreElicitationSessionRepository>();
                options.AddRepository<ElicitationArtifact, EfCoreElicitationArtifactRepository>();
                options.AddRepository<BrainstormingIdea, EfCoreBrainstormingIdeaRepository>();

                // Add default repositories for entities that don't have custom repositories
                options.AddDefaultRepository<ElicitationSessionParticipant>();
                options.AddDefaultRepository<SurveyDefinition>();
                options.AddDefaultRepository<SurveyResponse>();
                options.AddDefaultRepository<InterviewRecord>();
            });

            // Configure EF Core options
            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql(); // Configure to use PostgreSQL
            });
        }
    }
}
