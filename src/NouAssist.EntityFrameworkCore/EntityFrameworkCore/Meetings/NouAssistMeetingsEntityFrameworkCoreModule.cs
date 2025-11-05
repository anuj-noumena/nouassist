using Microsoft.Extensions.DependencyInjection;
using NouAssist.Meetings.Aggregates;
using NouAssist.Meetings.Data;
using NouAssist.Meetings.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSQL;
using Volo.Abp.Modularity;

namespace NouAssist.Meetings.EntityFrameworkCore
{
    [DependsOn(
        typeof(NouAssistMeetingsDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSQLModule)
    )]
    public class NouAssistMeetingsEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<MeetingsDbContext>(options =>
            {
                options.AddRepository<Meeting, EfCoreMeetingRepository>();

                options.AddDefaultRepositories(includeAllEntities: true);
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }
    }
}
