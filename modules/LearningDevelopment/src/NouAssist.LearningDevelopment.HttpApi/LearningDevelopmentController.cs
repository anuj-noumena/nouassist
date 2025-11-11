using NouAssist.LearningDevelopment.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.LearningDevelopment;

public abstract class LearningDevelopmentController : AbpControllerBase
{
    protected LearningDevelopmentController()
    {
        LocalizationResource = typeof(LearningDevelopmentResource);
    }
}
