using NouAssist.CoreProductivity.HabitSystems.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.CoreProductivity.HabitSystems.Permissions;

public class HabitSystemsPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(HabitSystemsPermissions.GroupName, L("Permission:HabitSystems"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<HabitSystemsResource>(name);
    }
}
