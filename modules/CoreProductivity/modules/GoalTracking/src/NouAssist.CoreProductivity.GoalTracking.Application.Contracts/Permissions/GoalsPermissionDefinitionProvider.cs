using NouAssist.CoreProductivity.GoalTracking.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.CoreProductivity.GoalTracking.Permissions;

public class GoalsPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(GoalsPermissions.GroupName, L("Permission:Goals"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<GoalTrackingResource>(name);
    }
}
