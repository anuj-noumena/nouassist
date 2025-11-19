using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace NouAssist.FinancialManagement.InvestmentManagement
{
    [DependsOn(
        typeof(InvestmentManagementDomainModule),
        typeof(InvestmentManagementApplicationContractsModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpFluentValidationModule)
    )]
    public class InvestmentManagementApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<InvestmentManagementApplicationModule>();
            
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<InvestmentManagementApplicationModule>(validate: true);
            });
        }
    }
}
