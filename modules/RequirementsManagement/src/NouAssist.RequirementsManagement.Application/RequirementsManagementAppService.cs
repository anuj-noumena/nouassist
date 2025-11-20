using NouAssist.RequirementsManagement.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.RequirementsManagement;

public abstract class RequirementsManagementAppService : ApplicationService
{
    protected RequirementsManagementAppService()
    {
        LocalizationResource = typeof(RequirementsManagementResource);
        ObjectMapperContext = typeof(RequirementsManagementApplicationModule);
    }
}
