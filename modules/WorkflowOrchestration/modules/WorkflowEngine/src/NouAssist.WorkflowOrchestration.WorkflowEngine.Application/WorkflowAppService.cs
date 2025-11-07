using NouAssist.WorkflowOrchestration.WorkflowEngine.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.WorkflowOrchestration.WorkflowEngine;

public abstract class WorkflowAppService : ApplicationService
{
    protected WorkflowAppService()
    {
        LocalizationResource = typeof(WorkflowEngineResource);
        ObjectMapperContext = typeof(WorkflowEngineApplicationModule);
    }
}
