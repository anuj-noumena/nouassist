using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace NouAssist.FinancialManagement.CreditScoreMonitoring
{
    [DependsOn(
        typeof(CreditScoreMonitoringDomainModule),
        typeof(CreditScoreMonitoringApplicationContractsModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpFluentValidationModule)
    )]
    public class CreditScoreMonitoringApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<CreditScoreMonitoringApplicationModule>();
            
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<CreditScoreMonitoringApplicationModule>(validate: true);
            });
        }
    }
}
