using NouAssist.CustomFields.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace NouAssist.CustomFields;

public abstract class CustomFieldsController : AbpControllerBase
{
    protected CustomFieldsController()
    {
        LocalizationResource = typeof(CustomFieldsResource);
    }
}
