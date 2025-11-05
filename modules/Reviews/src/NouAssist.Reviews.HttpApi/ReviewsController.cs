using NouAssist.Reviews.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.Reviews;

public abstract class ReviewsController : AbpControllerBase
{
    protected ReviewsController()
    {
        LocalizationResource = typeof(ReviewsResource);
    }
}
