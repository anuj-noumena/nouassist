using Volo.Abp.Modularity;

namespace NouAssist.Approval;

[DependsOn(
    typeof(ApprovalApplicationModule),
    typeof(ApprovalDomainTestModule)
    )]
public class ApprovalApplicationTestModule : AbpModule
{

}
