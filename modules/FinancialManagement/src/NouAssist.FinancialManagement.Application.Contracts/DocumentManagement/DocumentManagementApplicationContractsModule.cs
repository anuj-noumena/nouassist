using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace NouAssist.DocumentManagement
{
    [DependsOn(
        typeof(DocumentManagementDomainSharedModule),
        typeof(AbpAuthorizationModule)
    )]
    public class DocumentManagementApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // Configure services if needed
        }
    }
}
