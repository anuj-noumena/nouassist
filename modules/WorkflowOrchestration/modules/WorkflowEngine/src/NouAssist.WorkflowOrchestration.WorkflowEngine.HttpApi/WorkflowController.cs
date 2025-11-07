using NouAssist.WorkflowOrchestration.WorkflowEngine.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.WorkflowOrchestration.WorkflowEngine;

public abstract class WorkflowController : AbpControllerBase
{
    protected WorkflowController()
    {
        LocalizationResource = typeof(WorkflowEngineResource);
    }
}
