using Volo.Abp.Authorization;
using Volo.Abp.FluentValidation;
using Volo.Abp.Modularity;

namespace NouAssist.Meetings
{
    [DependsOn(
        typeof(NouAssistMeetingsDomainSharedModule),
        typeof(AbpAuthorizationModule),
        typeof(AbpFluentValidationModule)
    )]
    public class NouAssistMeetingsApplicationContractsModule : AbpModule
    {
    }
}
