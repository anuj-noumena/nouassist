using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace NouAssist.FinancialManagement.TaxManagement
{
    [DependsOn(
        typeof(TaxManagementDomainModule),
        typeof(TaxManagementApplicationContractsModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpFluentValidationModule)
    )]
    public class TaxManagementApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<TaxManagementApplicationModule>();
            
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<TaxManagementApplicationModule>(validate: true);
            });
        }
    }
}
