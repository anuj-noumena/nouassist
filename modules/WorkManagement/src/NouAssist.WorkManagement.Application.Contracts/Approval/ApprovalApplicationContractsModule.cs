using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace NouAssist.WorkManagement.Approval;

[DependsOn(
    typeof(ApprovalDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class ApprovalApplicationContractsModule : AbpModule
{

}
