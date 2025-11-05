using NouAssist.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace NouAssist.Permissions;

public class NouAssistPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(NouAssistPermissions.GroupName);

        //Define your own permissions here. Example:
        //myGroup.AddPermission(NouAssistPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<NouAssistResource>(name);
    }
}
