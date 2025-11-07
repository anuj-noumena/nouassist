using NouAssist.ReflectionAndSupport.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.ReflectionAndSupport;

public abstract class ReflectionAndSupportAppService : ApplicationService
{
    protected ReflectionAndSupportAppService()
    {
        LocalizationResource = typeof(ReflectionAndSupportResource);
        ObjectMapperContext = typeof(ReflectionAndSupportApplicationModule);
    }
}
