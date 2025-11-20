using NouAssist.RequirementsManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.RequirementsManagement.Permissions;

public class RequirementsManagementPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(RequirementsManagementPermissions.GroupName, L("Permission:RequirementsManagement"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<RequirementsManagementResource>(name);
    }
}
