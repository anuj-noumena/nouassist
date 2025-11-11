using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;

namespace NouAssist.Users;

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
