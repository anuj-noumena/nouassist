using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.Approval;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(ApprovalDomainSharedModule)
)]
public class ApprovalDomainModule : AbpModule
{

}
