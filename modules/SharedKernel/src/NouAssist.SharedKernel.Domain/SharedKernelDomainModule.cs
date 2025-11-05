using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.SharedKernel;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(SharedKernelDomainSharedModule)
)]
public class SharedKernelDomainModule : AbpModule
{

}
