using NouAssist.AssetManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.AssetManagement;

public abstract class AssetManagementController : AbpControllerBase
{
    protected AssetManagementController()
    {
        LocalizationResource = typeof(AssetManagementResource);
    }
}
