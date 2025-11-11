using Microsoft.Extensions.DependencyInjection;
using NouAssist.PersonalDevelopment.Goals.Aggregates;
using NouAssist.PersonalDevelopment.Goals.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.PersonalDevelopment.Goals.EntityFrameworkCore
{
    [DependsOn(
        typeof(GoalsDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSqlModule)
    )]
    public class GoalsEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<GoalsDbContext>(options =>
            {
                options.AddRepository<Goal, EfCoreGoalRepository>();
                options.AddRepository<Achievement, EfCoreAchievementRepository>();
                options.AddRepository<UserAchievement, EfCoreUserAchievementRepository>();
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }
    }
}
