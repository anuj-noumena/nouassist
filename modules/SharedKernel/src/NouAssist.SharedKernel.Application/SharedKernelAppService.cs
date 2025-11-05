using NouAssist.SharedKernel.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.SharedKernel;

public abstract class SharedKernelAppService : ApplicationService
{
    protected SharedKernelAppService()
    {
        LocalizationResource = typeof(SharedKernelResource);
        ObjectMapperContext = typeof(SharedKernelApplicationModule);
    }
}
