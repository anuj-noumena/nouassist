using NouAssist.IntelligenceStrategy.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.IntelligenceStrategy;

public abstract class IntelligenceStrategyController : AbpControllerBase
{
    protected IntelligenceStrategyController()
    {
        LocalizationResource = typeof(IntelligenceStrategyResource);
    }
}
