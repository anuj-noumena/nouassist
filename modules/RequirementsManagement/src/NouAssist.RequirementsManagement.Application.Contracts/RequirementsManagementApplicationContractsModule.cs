using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace NouAssist.RequirementsManagement;

[DependsOn(
    typeof(RequirementsManagementDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class RequirementsManagementApplicationContractsModule : AbpModule
{

}
