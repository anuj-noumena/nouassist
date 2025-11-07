using Volo.Abp.Modularity;

namespace NouAssist.Infrastructure;

[DependsOn(
    typeof(InfrastructureApplicationModule),
    typeof(InfrastructureDomainTestModule)
    )]
public class InfrastructureApplicationTestModule : AbpModule
{

}
