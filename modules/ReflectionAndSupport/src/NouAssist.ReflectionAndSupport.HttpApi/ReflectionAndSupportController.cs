using NouAssist.ReflectionAndSupport.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.ReflectionAndSupport;

public abstract class ReflectionAndSupportController : AbpControllerBase
{
    protected ReflectionAndSupportController()
    {
        LocalizationResource = typeof(ReflectionAndSupportResource);
    }
}
