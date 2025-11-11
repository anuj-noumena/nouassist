using NouAssist.WorkManagement.Approval.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.WorkManagement.Approval;

public abstract class ApprovalAppService : ApplicationService
{
    protected ApprovalAppService()
    {
        LocalizationResource = typeof(ApprovalResource);
        ObjectMapperContext = typeof(ApprovalApplicationModule);
    }
}
