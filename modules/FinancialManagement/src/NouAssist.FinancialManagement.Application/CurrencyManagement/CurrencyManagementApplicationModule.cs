using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace NouAssist.FinancialManagement.CurrencyManagement
{
    [DependsOn(
        typeof(CurrencyManagementDomainModule),
        typeof(CurrencyManagementApplicationContractsModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpFluentValidationModule)
    )]
    public class CurrencyManagementApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<CurrencyManagementApplicationModule>();
            
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<CurrencyManagementApplicationModule>(validate: true);
            });
        }
    }
}
