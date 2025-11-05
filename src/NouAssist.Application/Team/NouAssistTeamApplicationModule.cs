using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace NouAssist.Team
{
    [DependsOn(
        typeof(NouAssistTeamDomainModule),
        typeof(NouAssistTeamApplicationContractsModule),
        typeof(AbpAutoMapperModule)
    )]
    public class NouAssistTeamApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<NouAssistTeamApplicationModule>();
            
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<NouAssistTeamApplicationModule>(validate: true);
            });
        }
    }
}
