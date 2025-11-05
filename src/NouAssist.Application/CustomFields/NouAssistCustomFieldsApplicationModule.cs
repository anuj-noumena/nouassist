using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace NouAssist.CustomFields
{
    [DependsOn(
        typeof(NouAssistCustomFieldsDomainModule),
        typeof(NouAssistCustomFieldsApplicationContractsModule),
        typeof(AbpAutoMapperModule)
    )]
    public class NouAssistCustomFieldsApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<NouAssistCustomFieldsApplicationModule>();
            
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<NouAssistCustomFieldsApplicationModule>(validate: true);
            });
        }
    }
}
