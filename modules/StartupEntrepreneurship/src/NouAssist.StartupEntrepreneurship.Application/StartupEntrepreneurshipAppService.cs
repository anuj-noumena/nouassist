using NouAssist.StartupEntrepreneurship.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.StartupEntrepreneurship;

public abstract class StartupEntrepreneurshipAppService : ApplicationService
{
    protected StartupEntrepreneurshipAppService()
    {
        LocalizationResource = typeof(StartupEntrepreneurshipResource);
        ObjectMapperContext = typeof(StartupEntrepreneurshipApplicationModule);
    }
}
