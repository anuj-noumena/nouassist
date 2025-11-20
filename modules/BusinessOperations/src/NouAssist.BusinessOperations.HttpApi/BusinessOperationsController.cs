using NouAssist.BusinessOperations.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.BusinessOperations;

public abstract class BusinessOperationsController : AbpControllerBase
{
    protected BusinessOperationsController()
    {
        LocalizationResource = typeof(BusinessOperationsResource);
    }
}
