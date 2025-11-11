using NouAssist.WorkManagement.GTD.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.WorkManagement.GTD;

public abstract class GTDAppService : ApplicationService
{
    protected GTDAppService()
    {
        LocalizationResource = typeof(GTDResource);
        ObjectMapperContext = typeof(GTDApplicationModule);
    }
}
