using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;

namespace NouAssist.LearningDevelopment;

[DependsOn(
    typeof(LearningDevelopmentDomainModule),
    typeof(LearningDevelopmentApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule)
    )]
public class LearningDevelopmentApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<LearningDevelopmentApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<LearningDevelopmentApplicationModule>(validate: true);
        });
    }
}
