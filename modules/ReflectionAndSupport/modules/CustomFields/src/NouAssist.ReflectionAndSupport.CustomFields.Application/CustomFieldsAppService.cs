using NouAssist.ReflectionAndSupport.CustomFields.Localization;
using Volo.Abp.Application.Services;

namespace NouAssist.ReflectionAndSupport.CustomFields;

public abstract class CustomFieldsAppService : ApplicationService
{
    protected CustomFieldsAppService()
    {
        LocalizationResource = typeof(CustomFieldsResource);
        ObjectMapperContext = typeof(CustomFieldsApplicationModule);
    }
}
