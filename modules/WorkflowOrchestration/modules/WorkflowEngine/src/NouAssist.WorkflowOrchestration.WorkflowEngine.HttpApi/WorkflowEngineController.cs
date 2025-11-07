using NouAssist.WorkflowOrchestration.WorkflowEngine.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.WorkflowOrchestration.WorkflowEngine;

public abstract class WorkflowEngineController : AbpControllerBase
{
    protected WorkflowEngineController()
    {
        LocalizationResource = typeof(WorkflowEngineResource);
    }
}
