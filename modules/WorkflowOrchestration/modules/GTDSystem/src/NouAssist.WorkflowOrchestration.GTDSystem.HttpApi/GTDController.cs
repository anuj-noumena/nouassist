using NouAssist.WorkflowOrchestration.GTDSystem.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.WorkflowOrchestration.GTDSystem;

public abstract class GTDController : AbpControllerBase
{
    protected GTDController()
    {
        LocalizationResource = typeof(GTDSystemResource);
    }
}
