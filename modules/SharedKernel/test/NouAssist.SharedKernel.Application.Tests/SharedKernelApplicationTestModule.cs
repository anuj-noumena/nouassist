using Volo.Abp.Modularity;

namespace NouAssist.SharedKernel;

[DependsOn(
    typeof(SharedKernelApplicationModule),
    typeof(SharedKernelDomainTestModule)
    )]
public class SharedKernelApplicationTestModule : AbpModule
{

}
