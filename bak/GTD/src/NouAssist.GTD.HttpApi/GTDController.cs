using NouAssist.GTD.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.GTD;

public abstract class GTDController : AbpControllerBase
{
    protected GTDController()
    {
        LocalizationResource = typeof(GTDResource);
    }
}
