using Volo.Abp.Authorization;
using Volo.Abp.FluentValidation;
using Volo.Abp.Modularity;

namespace NouAssist.Approval
{
    [DependsOn(
        typeof(NouAssistApprovalDomainSharedModule),
        typeof(AbpAuthorizationModule),
        typeof(AbpFluentValidationModule)
    )]
    public class NouAssistApprovalApplicationContractsModule : AbpModule
    {
    }
}
