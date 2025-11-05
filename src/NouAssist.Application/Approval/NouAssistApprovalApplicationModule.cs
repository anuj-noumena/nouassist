using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace NouAssist.Approval
{
    [DependsOn(
        typeof(NouAssistApprovalDomainModule),
        typeof(NouAssistApprovalApplicationContractsModule),
        typeof(AbpAutoMapperModule)
    )]
    public class NouAssistApprovalApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<NouAssistApprovalApplicationModule>();
            
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<NouAssistApprovalApplicationModule>(validate: true);
            });
        }
    }
}
