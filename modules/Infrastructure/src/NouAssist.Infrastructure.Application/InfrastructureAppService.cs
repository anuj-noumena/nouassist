using NouAssist.Infrastructure.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.Infrastructure;

public abstract class InfrastructureAppService : ApplicationService
{
    protected InfrastructureAppService()
    {
        LocalizationResource = typeof(InfrastructureResource);
        ObjectMapperContext = typeof(InfrastructureApplicationModule);
    }
}
