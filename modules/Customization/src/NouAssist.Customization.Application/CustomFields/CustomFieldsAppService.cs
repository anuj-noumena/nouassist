using NouAssist.Customization.CustomFields.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.Customization.CustomFields;

public abstract class CustomFieldsAppService : ApplicationService
{
    protected CustomFieldsAppService()
    {
        LocalizationResource = typeof(CustomFieldsResource);
        ObjectMapperContext = typeof(CustomFieldsApplicationModule);
    }
}
