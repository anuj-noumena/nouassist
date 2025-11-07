using NouAssist.CollaborationHub.DiscussionThreads.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.CollaborationHub.DiscussionThreads;

public abstract class DiscussionThreadsAppService : ApplicationService
{
    protected DiscussionThreadsAppService()
    {
        LocalizationResource = typeof(DiscussionThreadsResource);
        ObjectMapperContext = typeof(DiscussionThreadsApplicationModule);
    }
}
