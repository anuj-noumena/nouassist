using Volo.Abp.Modularity;

namespace NouAssist.SharedKernel;

[DependsOn(
    typeof(SharedKernelDomainModule),
    typeof(SharedKernelTestBaseModule)
)]
public class SharedKernelDomainTestModule : AbpModule
{

}
