using Volo.Abp.Modularity;

namespace NouAssist.Meetings;

[DependsOn(
    typeof(MeetingsDomainModule),
    typeof(MeetingsTestBaseModule)
)]
public class MeetingsDomainTestModule : AbpModule
{

}
