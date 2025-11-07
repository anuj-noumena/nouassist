using Volo.Abp.Modularity;

namespace NouAssist.CollaborationHub.DiscussionThreads;

[DependsOn(
    typeof(DiscussionThreadsDomainModule),
    typeof(DiscussionThreadsTestBaseModule)
)]
public class DiscussionThreadsDomainTestModule : AbpModule
{

}
