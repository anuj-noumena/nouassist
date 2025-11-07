using NouAssist.ReflectionAndSupport.Journaling.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.ReflectionAndSupport.Journaling;

public abstract class JournalingController : AbpControllerBase
{
    protected JournalingController()
    {
        LocalizationResource = typeof(JournalingResource);
    }
}
