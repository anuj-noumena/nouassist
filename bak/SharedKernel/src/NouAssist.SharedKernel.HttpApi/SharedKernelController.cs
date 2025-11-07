using NouAssist.SharedKernel.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.SharedKernel;

public abstract class SharedKernelController : AbpControllerBase
{
    protected SharedKernelController()
    {
        LocalizationResource = typeof(SharedKernelResource);
    }
}
