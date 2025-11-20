using Microsoft.Extensions.DependencyInjection;
using NouAssist.TimeManagement.Calendar.Aggregates;
using NouAssist.TimeManagement.EntityFrameworkCore.Calendar.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.TimeManagement.EntityFrameworkCore;

[DependsOn(
    typeof(TimeManagementDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class TimeManagementEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<TimeManagementDbContext>(options =>
        {
            options.AddRepository<TimeManagement.Calendar.Aggregates.Calendar, EfCoreCalendarRepository>();
            options.AddRepository<CalendarEvent, EfCoreCalendarEventRepository>();
            options.AddRepository<Timeblock, EfCoreTimeblockRepository>();
            options.AddRepository<SchedulingRequest, EfCoreSchedulingRequestRepository>();
            options.AddRepository<TimeEntry, EfCoreTimeEntryRepository>();
            options.AddRepository<TimeblockTemplate, EfCoreTimeblockTemplateRepository>();
            options.AddRepository<UserAvailability, EfCoreUserAvailabilityRepository>();
            
            options.AddDefaultRepositories<ITimeManagementDbContext>(includeAllEntities: true);
            
        });
    }
}
