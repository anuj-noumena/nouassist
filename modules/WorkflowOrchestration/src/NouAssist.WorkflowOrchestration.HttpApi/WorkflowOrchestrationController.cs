using NouAssist.WorkflowOrchestration.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.WorkflowOrchestration;

public abstract class WorkflowOrchestrationController : AbpControllerBase
{
    protected WorkflowOrchestrationController()
    {
        LocalizationResource = typeof(WorkflowOrchestrationResource);
    }
}
