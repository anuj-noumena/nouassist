using Microsoft.Extensions.DependencyInjection;
using NouAssist.IdentityAccess.Users.Aggregates;
using NouAssist.IdentityAccess.Users.EntityFrameworkCore.Repositories;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.IdentityAccess.Users.EntityFrameworkCore
{
    [DependsOn(
        typeof(UsersDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSqlModule)
    )]
    public class UsersEntityFrameworkCoreModule : AbpModule
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
