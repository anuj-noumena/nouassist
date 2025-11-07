using NouAssist.Infrastructure.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.Infrastructure;

public abstract class InfrastructureController : AbpControllerBase
{
    protected InfrastructureController()
    {
        LocalizationResource = typeof(InfrastructureResource);
    }
}
