using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.Tasks;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(TasksDomainSharedModule)
)]
public class TasksDomainModule : AbpModule
{

}
