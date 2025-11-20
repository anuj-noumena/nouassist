using NouAssist.BusinessOperations.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.BusinessOperations;

public abstract class BusinessOperationsAppService : ApplicationService
{
    protected BusinessOperationsAppService()
    {
        LocalizationResource = typeof(BusinessOperationsResource);
        ObjectMapperContext = typeof(BusinessOperationsApplicationModule);
    }
}
