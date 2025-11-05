using Volo.Abp.Authorization;
using Volo.Abp.FluentValidation;
using Volo.Abp.Modularity;

namespace NouAssist.Team
{
    [DependsOn(
        typeof(NouAssistTeamDomainSharedModule),
        typeof(AbpAuthorizationModule),
        typeof(AbpFluentValidationModule)
    )]
    public class NouAssistTeamApplicationContractsModule : AbpModule
    {
    }
}
