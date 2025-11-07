using NouAssist.ReflectionAndSupport.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.ReflectionAndSupport.Permissions;

public class ReflectionAndSupportPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(ReflectionAndSupportPermissions.GroupName, L("Permission:ReflectionAndSupport"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ReflectionAndSupportResource>(name);
    }
}
