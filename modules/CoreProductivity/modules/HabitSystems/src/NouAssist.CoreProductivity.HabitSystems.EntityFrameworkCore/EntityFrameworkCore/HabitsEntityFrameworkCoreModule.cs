using Microsoft.Extensions.DependencyInjection;
using NouAssist.CoreProductivity.HabitSystems.Aggregates;
using NouAssist.CoreProductivity.HabitSystems.Data;
using NouAssist.CoreProductivity.HabitSystems.Entities;
using NouAssist.CoreProductivity.HabitSystems.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSQL;
using Volo.Abp.Modularity;

namespace NouAssist.CoreProductivity.HabitSystems.EntityFrameworkCore
{
    [DependsOn(
        typeof(NouAssistHabitSystemsDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSQLModule)
    )]
    public class HabitSystemsEntityFrameworkCoreModule : AbpModule
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
