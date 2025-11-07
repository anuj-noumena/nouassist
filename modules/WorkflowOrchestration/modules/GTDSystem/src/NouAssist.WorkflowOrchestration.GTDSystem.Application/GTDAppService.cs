using NouAssist.WorkflowOrchestration.GTDSystem.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.WorkflowOrchestration.GTDSystem;

public abstract class GTDAppService : ApplicationService
{
    protected GTDAppService()
    {
        LocalizationResource = typeof(GTDSystemResource);
        ObjectMapperContext = typeof(GTDSystemApplicationModule);
    }
}
