using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace NouAssist.CollaborationHub.DiscussionThreads;

[DependsOn(
    typeof(DiscussionThreadsDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class DiscussionThreadsApplicationContractsModule : AbpModule
{

}
