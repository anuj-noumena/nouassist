using NouAssist.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist;

/* Inherit your application services from this class.
 */
public abstract class NouAssistAppService : ApplicationService
{
    protected NouAssistAppService()
    {
        LocalizationResource = typeof(NouAssistResource);
    }
}
