using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.Application;

namespace NouAssist.PersonalDevelopment;

[DependsOn(
    typeof(PersonalDevelopmentDomainModule),
    typeof(PersonalDevelopmentApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule)
    )]
public class PersonalDevelopmentApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<PersonalDevelopmentApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<PersonalDevelopmentApplicationModule>(validate: true);
        });
    }
}
