using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace NouAssist.FinancialManagement.AuditTrail
{
    [DependsOn(
        typeof(AuditTrailDomainModule),
        typeof(AuditTrailApplicationContractsModule),
        typeof(AbpAutoMapperModule)
    )]
    public class AuditTrailApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<AuditTrailApplicationModule>();
            
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<AuditTrailApplicationModule>(validate: true);
            });
        }
    }
}
