using NouAssist.LearningDevelopment.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.LearningDevelopment;

public abstract class LearningDevelopmentAppService : ApplicationService
{
    protected LearningDevelopmentAppService()
    {
        LocalizationResource = typeof(LearningDevelopmentResource);
        ObjectMapperContext = typeof(LearningDevelopmentApplicationModule);
    }
}
