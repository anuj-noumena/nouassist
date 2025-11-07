using NouAssist.CollaborationHub.DiscussionThreads.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.CollaborationHub.DiscussionThreads;

public abstract class CommentsController : AbpControllerBase
{
    protected CommentsController()
    {
        LocalizationResource = typeof(DiscussionThreadsResource);
    }
}
