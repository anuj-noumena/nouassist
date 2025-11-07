using Volo.Abp.Modularity;

namespace NouAssist.GTD;

[DependsOn(
    typeof(GTDApplicationModule),
    typeof(GTDDomainTestModule)
    )]
public class GTDApplicationTestModule : AbpModule
{

}
