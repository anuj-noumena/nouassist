using NouAssist.GTD.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.GTD;

public abstract class GTDAppService : ApplicationService
{
    protected GTDAppService()
    {
        LocalizationResource = typeof(GTDResource);
        ObjectMapperContext = typeof(GTDApplicationModule);
    }
}
