using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace NouAssist.IdentityAccess.Team;

[DependsOn(
    typeof(TeamDomainModule),
    typeof(TeamApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule)
    )]
public class TeamApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<TeamApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<TeamApplicationModule>(validate: true);
        });
    }
}
