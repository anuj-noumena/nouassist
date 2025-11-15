using Volo.Abp.Modularity;

namespace NouAssist.AuditTrail
{
    [DependsOn(
        typeof(AuditTrailApplicationModule),
        typeof(AuditTrailDomainTestModule)
    )]
    public class AuditTrailApplicationTestModule : AbpModule
    {
    }
}
