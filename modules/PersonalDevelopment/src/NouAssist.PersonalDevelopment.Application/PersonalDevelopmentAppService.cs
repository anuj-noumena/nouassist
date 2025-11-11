using NouAssist.PersonalDevelopment.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.PersonalDevelopment;

public abstract class PersonalDevelopmentAppService : ApplicationService
{
    protected PersonalDevelopmentAppService()
    {
        LocalizationResource = typeof(PersonalDevelopmentResource);
        ObjectMapperContext = typeof(PersonalDevelopmentApplicationModule);
    }
}
