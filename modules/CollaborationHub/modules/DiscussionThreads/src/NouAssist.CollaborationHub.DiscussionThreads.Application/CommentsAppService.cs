using NouAssist.CollaborationHub.DiscussionThreads.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.CollaborationHub.DiscussionThreads;

public abstract class CommentsAppService : ApplicationService
{
    protected CommentsAppService()
    {
        LocalizationResource = typeof(DiscussionThreadsResource);
        ObjectMapperContext = typeof(DiscussionThreadsApplicationModule);
    }
}
