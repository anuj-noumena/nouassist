using NouAssist.CoreProductivity.TimeManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.CoreProductivity.TimeManagement.Permissions;

public class TimeManagementPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(TimeManagementPermissions.GroupName, L("Permission:TimeManagement"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<TimeManagementResource>(name);
    }
}
