using NouAssist.WorkManagement.Workflow.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.WorkManagement.Workflow;

public abstract class WorkflowAppService : ApplicationService
{
    protected WorkflowAppService()
    {
        LocalizationResource = typeof(WorkflowResource);
        ObjectMapperContext = typeof(WorkflowApplicationModule);
    }
}
