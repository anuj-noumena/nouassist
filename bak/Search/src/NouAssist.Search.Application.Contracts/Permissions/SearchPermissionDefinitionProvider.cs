using NouAssist.Search.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.Search.Permissions;

public class SearchPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(SearchPermissions.GroupName, L("Permission:Search"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<SearchResource>(name);
    }
}
