using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.CollaborationHub.DiscussionThreads;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(DiscussionThreadsDomainSharedModule)
)]
public class DiscussionThreadsDomainModule : AbpModule
{

}
