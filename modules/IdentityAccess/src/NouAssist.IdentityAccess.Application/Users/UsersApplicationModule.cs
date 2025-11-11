using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace NouAssist.IdentityAccess.Users;

[DependsOn(
    typeof(UsersDomainModule),
    typeof(UsersApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule)
    )]
public class UsersApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<UsersApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<UsersApplicationModule>(validate: true);
        });
    }
}
