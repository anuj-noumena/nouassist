using NouAssist.Customization.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.Customization;

public abstract class CustomizationController : AbpControllerBase
{
    protected CustomizationController()
    {
        LocalizationResource = typeof(CustomizationResource);
    }
}
