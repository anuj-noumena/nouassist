using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace NouAssist.FinancialManagement.DebtManagement
{
    [DependsOn(
        typeof(DebtManagementDomainModule),
        typeof(DebtManagementApplicationContractsModule),
        typeof(AbpAutoMapperModule),
        typeof(AbpFluentValidationModule)
    )]
    public class DebtManagementApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<DebtManagementApplicationModule>();
            
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<DebtManagementApplicationModule>(validate: true);
            });
        }
    }
}
