using NouAssist.FinancialManagement.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.FinancialManagement;

public abstract class FinancialManagementAppService : ApplicationService
{
    protected FinancialManagementAppService()
    {
        LocalizationResource = typeof(FinancialManagementResource);
        ObjectMapperContext = typeof(FinancialManagementApplicationModule);
    }
}
