using NouAssist.AssetManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.AssetManagement.Permissions;

public class AssetManagementPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(AssetManagementPermissions.GroupName, L("Permission:AssetManagement"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AssetManagementResource>(name);
    }
}
