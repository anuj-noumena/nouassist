using Volo.Abp.Modularity;

namespace NouAssist.CollaborationHub.DiscussionThreads;

[DependsOn(
    typeof(DiscussionThreadsApplicationModule),
    typeof(DiscussionThreadsDomainTestModule)
    )]
public class DiscussionThreadsApplicationTestModule : AbpModule
{

}
