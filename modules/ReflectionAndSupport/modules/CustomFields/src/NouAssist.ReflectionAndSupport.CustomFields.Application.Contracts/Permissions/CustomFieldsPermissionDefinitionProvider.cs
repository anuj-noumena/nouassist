using NouAssist.ReflectionAndSupport.CustomFields.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.ReflectionAndSupport.CustomFields.Permissions
{
    public class CustomFieldsPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var customFieldsGroup = context.AddGroup(
                CustomFieldsPermissions.GroupName,
                L("Permission:CustomFields"));

            var definitionsPermission = customFieldsGroup.AddPermission(
                CustomFieldsPermissions.Definitions.Default,
                L("Permission:CustomFieldDefinitions"));
            definitionsPermission.AddChild(
                CustomFieldsPermissions.Definitions.Create,
                L("Permission:CustomFieldDefinitions.Create"));
            definitionsPermission.AddChild(
                CustomFieldsPermissions.Definitions.Update,
                L("Permission:CustomFieldDefinitions.Update"));
            definitionsPermission.AddChild(
                CustomFieldsPermissions.Definitions.Delete,
                L("Permission:CustomFieldDefinitions.Delete"));
            definitionsPermission.AddChild(
                CustomFieldsPermissions.Definitions.Activate,
                L("Permission:CustomFieldDefinitions.Activate"));
            definitionsPermission.AddChild(
                CustomFieldsPermissions.Definitions.Deactivate,
                L("Permission:CustomFieldDefinitions.Deactivate"));

            var valuesPermission = customFieldsGroup.AddPermission(
                CustomFieldsPermissions.Values.Default,
                L("Permission:CustomFieldValues"));
            valuesPermission.AddChild(
                CustomFieldsPermissions.Values.Manage,
                L("Permission:CustomFieldValues.Manage"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<CustomFieldsResource>(name);
        }
    }
}
