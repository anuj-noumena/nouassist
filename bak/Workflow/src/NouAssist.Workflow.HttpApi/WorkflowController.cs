using NouAssist.Workflow.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.Workflow;

public abstract class WorkflowController : AbpControllerBase
{
    protected WorkflowController()
    {
        LocalizationResource = typeof(WorkflowResource);
    }
}
