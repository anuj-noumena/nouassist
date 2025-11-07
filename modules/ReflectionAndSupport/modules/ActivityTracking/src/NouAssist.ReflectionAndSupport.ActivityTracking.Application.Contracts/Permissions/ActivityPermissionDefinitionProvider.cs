using NouAssist.ReflectionAndSupport.ActivityTracking.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.ReflectionAndSupport.ActivityTracking.Permissions;

public class ActivityPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(ActivityPermissions.GroupName, L("Permission:Activity"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ActivityTrackingResource>(name);
    }
}
