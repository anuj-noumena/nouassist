using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace NouAssist.PersonalDevelopment.Goals;

[DependsOn(
    typeof(GoalsDomainModule),
    typeof(GoalsApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule)
    )]
public class GoalsApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<GoalsApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<GoalsApplicationModule>(validate: true);
        });
    }
}
