using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace NouAssist.Collaboration.Comments;

[DependsOn(
    typeof(CommentsDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class CommentsApplicationContractsModule : AbpModule
{

}
