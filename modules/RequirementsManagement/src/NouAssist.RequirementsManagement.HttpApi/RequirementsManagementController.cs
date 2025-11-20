using NouAssist.RequirementsManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.RequirementsManagement;

public abstract class RequirementsManagementController : AbpControllerBase
{
    protected RequirementsManagementController()
    {
        LocalizationResource = typeof(RequirementsManagementResource);
    }
}
