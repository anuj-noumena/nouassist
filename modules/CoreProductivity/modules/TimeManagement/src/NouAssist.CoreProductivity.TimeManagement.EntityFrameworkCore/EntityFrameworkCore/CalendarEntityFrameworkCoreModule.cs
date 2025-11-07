using Microsoft.Extensions.DependencyInjection;
using NouAssist.CoreProductivity.TimeManagement.Aggregates;
using NouAssist.CoreProductivity.TimeManagement.Data;
using NouAssist.CoreProductivity.TimeManagement.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSQL;
using Volo.Abp.Modularity;

namespace NouAssist.CoreProductivity.TimeManagement.EntityFrameworkCore
{
    [DependsOn(
        typeof(TimeManagementDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSQLModule)
    )]
    public class TimeManagementEntityFrameworkCoreModule : AbpModule
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
