using NouAssist.ReflectionAndSupport.CustomFields.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.ReflectionAndSupport.CustomFields.Permissions;

public class CustomFieldsPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(CustomFieldsPermissions.GroupName, L("Permission:CustomFields"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<CustomFieldsResource>(name);
    }
}
