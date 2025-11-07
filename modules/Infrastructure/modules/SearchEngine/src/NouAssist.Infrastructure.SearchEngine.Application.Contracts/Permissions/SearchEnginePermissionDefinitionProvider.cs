using NouAssist.Infrastructure.SearchEngine.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.Infrastructure.SearchEngine.Permissions;

public class SearchEnginePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(SearchEnginePermissions.GroupName, L("Permission:SearchEngine"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<SearchEngineResource>(name);
    }
}
