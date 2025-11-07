using NouAssist.Approval.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.Approval;

public abstract class ApprovalController : AbpControllerBase
{
    protected ApprovalController()
    {
        LocalizationResource = typeof(ApprovalResource);
    }
}
