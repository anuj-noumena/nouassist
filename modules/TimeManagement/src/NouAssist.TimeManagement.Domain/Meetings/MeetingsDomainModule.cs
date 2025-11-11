using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.Meetings;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(MeetingsDomainSharedModule)
)]
public class MeetingsDomainModule : AbpModule
{

}
