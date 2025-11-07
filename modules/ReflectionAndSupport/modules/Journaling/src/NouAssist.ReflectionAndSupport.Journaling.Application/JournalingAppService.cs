using NouAssist.ReflectionAndSupport.Journaling.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.ReflectionAndSupport.Journaling;

public abstract class JournalingAppService : ApplicationService
{
    protected JournalingAppService()
    {
        LocalizationResource = typeof(JournalingResource);
        ObjectMapperContext = typeof(JournalingApplicationModule);
    }
}
