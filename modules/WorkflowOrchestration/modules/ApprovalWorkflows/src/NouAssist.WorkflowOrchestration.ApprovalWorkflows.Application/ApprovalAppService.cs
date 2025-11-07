using NouAssist.WorkflowOrchestration.ApprovalWorkflows.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.WorkflowOrchestration.ApprovalWorkflows;

public abstract class ApprovalAppService : ApplicationService
{
    protected ApprovalAppService()
    {
        LocalizationResource = typeof(ApprovalWorkflowsResource);
        ObjectMapperContext = typeof(ApprovalWorkflowsApplicationModule);
    }
}
