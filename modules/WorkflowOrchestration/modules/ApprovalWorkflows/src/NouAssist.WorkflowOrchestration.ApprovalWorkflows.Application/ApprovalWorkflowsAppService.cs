using NouAssist.WorkflowOrchestration.ApprovalWorkflows.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.WorkflowOrchestration.ApprovalWorkflows;

public abstract class ApprovalWorkflowsAppService : ApplicationService
{
    protected ApprovalWorkflowsAppService()
    {
        LocalizationResource = typeof(ApprovalWorkflowsResource);
        ObjectMapperContext = typeof(ApprovalWorkflowsApplicationModule);
    }
}
