using Microsoft.Extensions.DependencyInjection;
using NouAssist.Calendar.Aggregates;
using NouAssist.Calendar.Data;
using NouAssist.Calendar.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSQL;
using Volo.Abp.Modularity;

namespace NouAssist.Calendar.EntityFrameworkCore
{
    [DependsOn(
        typeof(CalendarDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSQLModule)
    )]
    public class CalendarEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<CalendarDbContext>(options =>
            {
                // Register custom repositories
                options.AddRepository<Aggregates.Calendar, EfCoreCalendarRepository>();
                options.AddRepository<CalendarEvent, EfCoreCalendarEventRepository>();
                options.AddRepository<Timeblock, EfCoreTimeblockRepository>();
                options.AddRepository<SchedulingRequest, EfCoreSchedulingRequestRepository>();
                options.AddRepository<TimeEntry, EfCoreTimeEntryRepository>();
                options.AddRepository<TimeblockTemplate, EfCoreTimeblockTemplateRepository>();
                options.AddRepository<UserAvailability, EfCoreUserAvailabilityRepository>();

                // Enable default repositories for entities without custom repository
                options.AddDefaultRepositories(includeAllEntities: true);
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }
    }
}
