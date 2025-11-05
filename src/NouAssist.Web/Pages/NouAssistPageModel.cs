using NouAssist.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace NouAssist.Web.Pages;

public abstract class NouAssistPageModel : AbpPageModel
{
    protected NouAssistPageModel()
    {
        LocalizationResourceType = typeof(NouAssistResource);
    }
}
