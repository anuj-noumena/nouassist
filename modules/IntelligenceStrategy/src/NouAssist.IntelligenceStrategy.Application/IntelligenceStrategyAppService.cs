using NouAssist.IntelligenceStrategy.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.IntelligenceStrategy;

public abstract class IntelligenceStrategyAppService : ApplicationService
{
    protected IntelligenceStrategyAppService()
    {
        LocalizationResource = typeof(IntelligenceStrategyResource);
        ObjectMapperContext = typeof(IntelligenceStrategyApplicationModule);
    }
}
