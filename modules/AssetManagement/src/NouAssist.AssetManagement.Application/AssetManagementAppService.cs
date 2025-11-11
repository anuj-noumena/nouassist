using NouAssist.AssetManagement.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.AssetManagement;

public abstract class AssetManagementAppService : ApplicationService
{
    protected AssetManagementAppService()
    {
        LocalizationResource = typeof(AssetManagementResource);
        ObjectMapperContext = typeof(AssetManagementApplicationModule);
    }
}
