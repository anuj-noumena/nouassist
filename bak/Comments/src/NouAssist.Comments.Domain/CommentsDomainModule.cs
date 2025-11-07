using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace NouAssist.Comments;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(CommentsDomainSharedModule)
)]
public class CommentsDomainModule : AbpModule
{

}
