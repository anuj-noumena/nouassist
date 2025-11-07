using NouAssist.ReflectionAndSupport.Reviews.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.ReflectionAndSupport.Reviews;

public abstract class ReviewsController : AbpControllerBase
{
    protected ReviewsController()
    {
        LocalizationResource = typeof(ReviewsResource);
    }
}
