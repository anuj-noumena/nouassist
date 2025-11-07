using NouAssist.ReflectionAndSupport.ActivityTracking.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.ReflectionAndSupport.ActivityTracking.Permissions;

public class ActivityTrackingPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(ActivityTrackingPermissions.GroupName, L("Permission:ActivityTracking"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ActivityTrackingResource>(name);
    }
}
