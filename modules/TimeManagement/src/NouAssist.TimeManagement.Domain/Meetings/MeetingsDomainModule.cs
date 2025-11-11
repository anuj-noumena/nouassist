using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.TimeManagement.Meetings;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(MeetingsDomainSharedModule)
)]
public class MeetingsDomainModule : AbpModule
{

}
