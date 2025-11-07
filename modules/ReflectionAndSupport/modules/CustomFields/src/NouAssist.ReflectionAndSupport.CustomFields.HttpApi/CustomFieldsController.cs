using NouAssist.ReflectionAndSupport.CustomFields.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.ReflectionAndSupport.CustomFields;

public abstract class CustomFieldsController : AbpControllerBase
{
    protected CustomFieldsController()
    {
        LocalizationResource = typeof(CustomFieldsResource);
    }
}
