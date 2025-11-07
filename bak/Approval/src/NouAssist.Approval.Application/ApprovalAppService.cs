using NouAssist.Approval.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.Approval;

public abstract class ApprovalAppService : ApplicationService
{
    protected ApprovalAppService()
    {
        LocalizationResource = typeof(ApprovalResource);
        ObjectMapperContext = typeof(ApprovalApplicationModule);
    }
}
