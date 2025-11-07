using Microsoft.Extensions.DependencyInjection;
using NouAssist.CoreProductivity.GoalTracking.Aggregates;
using NouAssist.CoreProductivity.GoalTracking.Data;
using NouAssist.CoreProductivity.GoalTracking.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.Modularity;

namespace NouAssist.CoreProductivity.GoalTracking.EntityFrameworkCore
{
    [DependsOn(
        typeof(GoalTrackingDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSqlModule)
    )]
    public class GoalTrackingEntityFrameworkCoreModule : AbpModule
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
