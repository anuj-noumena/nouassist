using NouAssist.PersonalDevelopment.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.PersonalDevelopment;

public abstract class PersonalDevelopmentController : AbpControllerBase
{
    protected PersonalDevelopmentController()
    {
        LocalizationResource = typeof(PersonalDevelopmentResource);
    }
}
