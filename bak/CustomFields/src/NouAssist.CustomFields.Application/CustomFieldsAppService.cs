using NouAssist.CustomFields.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.CustomFields;

public abstract class CustomFieldsAppService : ApplicationService
{
    protected CustomFieldsAppService()
    {
        LocalizationResource = typeof(CustomFieldsResource);
        ObjectMapperContext = typeof(CustomFieldsApplicationModule);
    }
}
