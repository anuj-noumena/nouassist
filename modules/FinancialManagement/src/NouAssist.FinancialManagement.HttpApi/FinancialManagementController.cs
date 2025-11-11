using NouAssist.FinancialManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.FinancialManagement;

public abstract class FinancialManagementController : AbpControllerBase
{
    protected FinancialManagementController()
    {
        LocalizationResource = typeof(FinancialManagementResource);
    }
}
