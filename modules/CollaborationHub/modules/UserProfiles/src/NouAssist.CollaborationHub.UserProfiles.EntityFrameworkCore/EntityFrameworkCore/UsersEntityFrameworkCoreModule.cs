using Microsoft.Extensions.DependencyInjection;
using NouAssist.CollaborationHub.UserProfiles.Aggregates;
using NouAssist.CollaborationHub.UserProfiles.Data;
using NouAssist.CollaborationHub.UserProfiles.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.Modularity;

namespace NouAssist.CollaborationHub.UserProfiles.EntityFrameworkCore
{
    [DependsOn(
        typeof(UserProfilesDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSqlModule)
    )]
    public class UserProfilesEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<UsersDbContext>(options =>
            {
                options.AddRepository<User, EfCoreUserRepository>();
                options.AddRepository<UserSettings, EfCoreUserSettingsRepository>();
                options.AddRepository<UserEisenhowerPreference, EfCoreUserEisenhowerPreferenceRepository>();
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }
    }
}
