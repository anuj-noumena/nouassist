using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;
using Microsoft.Extensions.Localization;
using NouAssist.Localization;

namespace NouAssist.Web;

[Dependency(ReplaceServices = true)]
public class NouAssistBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<NouAssistResource> _localizer;

    public NouAssistBrandingProvider(IStringLocalizer<NouAssistResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
