using NouAssist.WorkflowOrchestration.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.WorkflowOrchestration;

public abstract class WorkflowOrchestrationAppService : ApplicationService
{
    protected WorkflowOrchestrationAppService()
    {
        LocalizationResource = typeof(WorkflowOrchestrationResource);
        ObjectMapperContext = typeof(WorkflowOrchestrationApplicationModule);
    }
}
