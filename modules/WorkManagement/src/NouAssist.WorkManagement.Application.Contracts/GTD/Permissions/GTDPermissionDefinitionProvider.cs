using NouAssist.WorkManagement.GTD.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.WorkManagement.GTD.Permissions;

public class GTDPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(GTDPermissions.GroupName, L("Permission:GTD"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<GTDResource>(name);
    }
}
