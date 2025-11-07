using NouAssist.ReflectionAndSupport.Reminders.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.ReflectionAndSupport.Reminders.Permissions;

public class RemindersPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(RemindersPermissions.GroupName, L("Permission:Reminders"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<RemindersResource>(name);
    }
}
