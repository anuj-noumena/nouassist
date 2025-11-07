using NouAssist.Comments.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.Comments;

public abstract class CommentsController : AbpControllerBase
{
    protected CommentsController()
    {
        LocalizationResource = typeof(CommentsResource);
    }
}
