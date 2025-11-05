using Volo.Abp.Modularity;

namespace NouAssist.Comments;

[DependsOn(
    typeof(CommentsDomainModule),
    typeof(CommentsTestBaseModule)
)]
public class CommentsDomainTestModule : AbpModule
{

}
