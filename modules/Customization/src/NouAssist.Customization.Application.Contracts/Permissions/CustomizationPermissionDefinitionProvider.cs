using NouAssist.Customization.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.Customization.Permissions;

public class CustomizationPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(CustomizationPermissions.GroupName, L("Permission:Customization"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<CustomizationResource>(name);
    }
}
