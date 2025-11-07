using NouAssist.WorkflowOrchestration.GTDSystem.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.WorkflowOrchestration.GTDSystem;

public abstract class GTDSystemAppService : ApplicationService
{
    protected GTDSystemAppService()
    {
        LocalizationResource = typeof(GTDSystemResource);
        ObjectMapperContext = typeof(GTDSystemApplicationModule);
    }
}
