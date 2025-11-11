using NouAssist.Customization.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.Customization.Permissions;

public class CustomizationPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(CustomizationPermissions.GroupName, L("Permission:Customization"));

        // CustomFields Permissions
        var definitionsPermission = myGroup.AddPermission(
            CustomizationPermissions.CustomFields.Definitions.Default,
            L("Permission:CustomFieldDefinitions"));
        definitionsPermission.AddChild(
            CustomizationPermissions.CustomFields.Definitions.Create,
            L("Permission:CustomFieldDefinitions.Create"));
        definitionsPermission.AddChild(
            CustomizationPermissions.CustomFields.Definitions.Update,
            L("Permission:CustomFieldDefinitions.Update"));
        definitionsPermission.AddChild(
            CustomizationPermissions.CustomFields.Definitions.Delete,
            L("Permission:CustomFieldDefinitions.Delete"));
        definitionsPermission.AddChild(
            CustomizationPermissions.CustomFields.Definitions.Activate,
            L("Permission:CustomFieldDefinitions.Activate"));
        definitionsPermission.AddChild(
            CustomizationPermissions.CustomFields.Definitions.Deactivate,
            L("Permission:CustomFieldDefinitions.Deactivate"));

        var valuesPermission = myGroup.AddPermission(
            CustomizationPermissions.CustomFields.Values.Default,
            L("Permission:CustomFieldValues"));
        valuesPermission.AddChild(
            CustomizationPermissions.CustomFields.Values.Manage,
            L("Permission:CustomFieldValues.Manage"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<CustomizationResource>(name);
    }
}
