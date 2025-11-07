using Volo.Abp.Modularity;

namespace NouAssist.Meetings;

[DependsOn(
    typeof(MeetingsApplicationModule),
    typeof(MeetingsDomainTestModule)
    )]
public class MeetingsApplicationTestModule : AbpModule
{

}
