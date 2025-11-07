using NouAssist.ReflectionAndSupport.Reviews.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.ReflectionAndSupport.Reviews;

public abstract class ReviewsAppService : ApplicationService
{
    protected ReviewsAppService()
    {
        LocalizationResource = typeof(ReviewsResource);
        ObjectMapperContext = typeof(ReviewsApplicationModule);
    }
}
