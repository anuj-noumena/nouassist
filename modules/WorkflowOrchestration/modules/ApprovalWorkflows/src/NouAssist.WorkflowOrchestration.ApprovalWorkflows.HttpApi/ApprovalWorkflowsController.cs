using NouAssist.WorkflowOrchestration.ApprovalWorkflows.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.WorkflowOrchestration.ApprovalWorkflows;

public abstract class ApprovalWorkflowsController : AbpControllerBase
{
    protected ApprovalWorkflowsController()
    {
        LocalizationResource = typeof(ApprovalWorkflowsResource);
    }
}
