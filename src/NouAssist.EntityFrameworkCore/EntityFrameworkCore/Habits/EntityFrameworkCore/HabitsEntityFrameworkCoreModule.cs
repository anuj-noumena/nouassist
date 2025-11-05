using Microsoft.Extensions.DependencyInjection;
using NouAssist.Habits.Aggregates;
using NouAssist.Habits.Data;
using NouAssist.Habits.Entities;
using NouAssist.Habits.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSQL;
using Volo.Abp.Modularity;

namespace NouAssist.Habits.EntityFrameworkCore
{
    [DependsOn(
        typeof(NouAssistHabitsDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSQLModule)
    )]
    public class HabitsEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<HabitsDbContext>(options =>
            {
                // Register custom repositories
                options.AddRepository<Habit, EfCoreHabitRepository>();
                options.AddRepository<HabitTrackingEntry, EfCoreHabitTrackingRepository>();
                options.AddRepository<Routine, EfCoreRoutineRepository>();
                options.AddRepository<RoutineCompletion, EfCoreRoutineCompletionRepository>();
                options.AddRepository<HealthTrackingEntry, EfCoreHealthTrackingRepository>();
                options.AddRepository<EnergyTrackingEntry, EfCoreEnergyTrackingRepository>();
                options.AddRepository<MoodTrackingEntry, EfCoreMoodTrackingRepository>();
                options.AddRepository<FocusSession, EfCoreFocusSessionRepository>();
                options.AddRepository<DailyReflection, EfCoreDailyReflectionRepository>();
                options.AddRepository<DailyIntention, EfCoreDailyIntentionRepository>();
                options.AddRepository<DailyQuote, EfCoreDailyQuoteRepository>();

                // Add default repositories for other entities if needed
                options.AddDefaultRepositories(includeAllEntities: false);
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }
    }
}
