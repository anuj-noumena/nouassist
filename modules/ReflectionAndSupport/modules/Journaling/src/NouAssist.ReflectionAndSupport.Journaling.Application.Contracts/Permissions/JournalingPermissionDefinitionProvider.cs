using NouAssist.ReflectionAndSupport.Journaling.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.ReflectionAndSupport.Journaling.Permissions;

public class JournalingPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(JournalingPermissions.GroupName, L("Permission:Journaling"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<JournalingResource>(name);
    }
}
