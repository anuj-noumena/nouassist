using NouAssist.WorkflowOrchestration.GTDSystem.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.WorkflowOrchestration.GTDSystem;

public abstract class GTDSystemController : AbpControllerBase
{
    protected GTDSystemController()
    {
        LocalizationResource = typeof(GTDSystemResource);
    }
}
