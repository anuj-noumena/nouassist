using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace NouAssist.CurrencyManagement
{
    [DependsOn(
        typeof(CurrencyManagementDomainSharedModule),
        typeof(AbpAuthorizationModule)
    )]
    public class CurrencyManagementApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // Configure services if needed
        }
    }
}
