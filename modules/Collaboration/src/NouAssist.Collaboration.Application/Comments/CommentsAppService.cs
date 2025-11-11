using NouAssist.Collaboration.Comments.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.Collaboration.Comments;

public abstract class CommentsAppService : ApplicationService
{
    protected CommentsAppService()
    {
        LocalizationResource = typeof(CommentsResource);
        ObjectMapperContext = typeof(CommentsApplicationModule);
    }
}
