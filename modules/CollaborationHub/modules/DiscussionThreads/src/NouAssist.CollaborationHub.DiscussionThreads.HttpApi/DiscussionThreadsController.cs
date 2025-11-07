using NouAssist.CollaborationHub.DiscussionThreads.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.CollaborationHub.DiscussionThreads;

public abstract class DiscussionThreadsController : AbpControllerBase
{
    protected DiscussionThreadsController()
    {
        LocalizationResource = typeof(DiscussionThreadsResource);
    }
}
