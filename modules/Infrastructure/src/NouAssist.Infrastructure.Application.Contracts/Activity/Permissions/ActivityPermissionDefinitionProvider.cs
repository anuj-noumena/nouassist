using NouAssist.Infrastructure.Activity.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.Infrastructure.Activity.Permissions;

public class ActivityPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(ActivityPermissions.GroupName, L("Permission:Activity"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ActivityResource>(name);
    }
}
