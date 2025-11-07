using NouAssist.WorkflowOrchestration.WorkflowEngine.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.WorkflowOrchestration.WorkflowEngine;

public abstract class WorkflowEngineAppService : ApplicationService
{
    protected WorkflowEngineAppService()
    {
        LocalizationResource = typeof(WorkflowEngineResource);
        ObjectMapperContext = typeof(WorkflowEngineApplicationModule);
    }
}
