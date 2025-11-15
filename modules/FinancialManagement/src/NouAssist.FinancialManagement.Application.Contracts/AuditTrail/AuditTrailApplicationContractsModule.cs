using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace NouAssist.AuditTrail
{
    [DependsOn(
        typeof(AuditTrailDomainSharedModule),
        typeof(AbpAuthorizationModule)
    )]
    public class AuditTrailApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // Configure services if needed
        }
    }
}
