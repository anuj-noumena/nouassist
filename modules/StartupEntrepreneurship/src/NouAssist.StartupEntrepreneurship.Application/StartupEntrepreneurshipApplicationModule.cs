using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;

namespace NouAssist.StartupEntrepreneurship;

[DependsOn(
    typeof(StartupEntrepreneurshipDomainModule),
    typeof(StartupEntrepreneurshipApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule)
    )]
public class StartupEntrepreneurshipApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<StartupEntrepreneurshipApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<StartupEntrepreneurshipApplicationModule>(validate: true);
        });
    }
}
