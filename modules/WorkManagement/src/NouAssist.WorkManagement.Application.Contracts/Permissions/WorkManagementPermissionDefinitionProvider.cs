using NouAssist.WorkManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.WorkManagement.Permissions;

public class WorkManagementPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(WorkManagementPermissions.GroupName, L("Permission:WorkManagement"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<WorkManagementResource>(name);
    }
}
