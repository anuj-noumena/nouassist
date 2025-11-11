using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.WorkManagement;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(WorkManagementDomainSharedModule)
)]
public class WorkManagementDomainModule : AbpModule
{

}
