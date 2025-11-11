using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace NouAssist.TimeManagement.Meetings;

[DependsOn(
    typeof(MeetingsDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class MeetingsApplicationContractsModule : AbpModule
{

}
