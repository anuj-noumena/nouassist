using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.RequirementsManagement;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(RequirementsManagementDomainSharedModule)
)]
public class RequirementsManagementDomainModule : AbpModule
{

}
