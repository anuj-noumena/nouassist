using NouAssist.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class NouAssistController : AbpControllerBase
{
    protected NouAssistController()
    {
        LocalizationResource = typeof(NouAssistResource);
    }
}
