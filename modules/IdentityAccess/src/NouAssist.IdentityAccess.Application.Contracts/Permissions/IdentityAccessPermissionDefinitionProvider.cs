using NouAssist.IdentityAccess.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.IdentityAccess.Permissions;

public class IdentityAccessPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(IdentityAccessPermissions.GroupName, L("Permission:IdentityAccess"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<IdentityAccessResource>(name);
    }
}
