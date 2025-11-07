using Volo.Abp.Modularity;

namespace NouAssist.Comments;

[DependsOn(
    typeof(CommentsApplicationModule),
    typeof(CommentsDomainTestModule)
    )]
public class CommentsApplicationTestModule : AbpModule
{

}
