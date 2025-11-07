using NouAssist.CoreProductivity.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.CoreProductivity.Permissions;

public class CoreProductivityPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(CoreProductivityPermissions.GroupName, L("Permission:CoreProductivity"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<CoreProductivityResource>(name);
    }
}
