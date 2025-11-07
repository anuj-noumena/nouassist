using NouAssist.WorkflowOrchestration.ApprovalWorkflows.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.WorkflowOrchestration.ApprovalWorkflows;

public abstract class ApprovalController : AbpControllerBase
{
    protected ApprovalController()
    {
        LocalizationResource = typeof(ApprovalWorkflowsResource);
    }
}
