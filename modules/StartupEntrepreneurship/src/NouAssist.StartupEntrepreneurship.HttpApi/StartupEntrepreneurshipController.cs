using NouAssist.StartupEntrepreneurship.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.StartupEntrepreneurship;

public abstract class StartupEntrepreneurshipController : AbpControllerBase
{
    protected StartupEntrepreneurshipController()
    {
        LocalizationResource = typeof(StartupEntrepreneurshipResource);
    }
}
