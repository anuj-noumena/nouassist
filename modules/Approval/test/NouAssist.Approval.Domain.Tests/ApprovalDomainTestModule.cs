using Volo.Abp.Modularity;

namespace NouAssist.Approval;

[DependsOn(
    typeof(ApprovalDomainModule),
    typeof(ApprovalTestBaseModule)
)]
public class ApprovalDomainTestModule : AbpModule
{

}
