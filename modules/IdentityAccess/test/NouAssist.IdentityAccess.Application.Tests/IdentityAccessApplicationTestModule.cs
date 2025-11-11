using Volo.Abp.Modularity;

namespace NouAssist.IdentityAccess;

[DependsOn(
    typeof(IdentityAccessApplicationModule),
    typeof(IdentityAccessDomainTestModule)
    )]
public class IdentityAccessApplicationTestModule : AbpModule
{

}
