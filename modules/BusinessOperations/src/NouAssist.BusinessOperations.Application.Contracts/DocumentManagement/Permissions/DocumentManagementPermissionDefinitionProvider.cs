using NouAssist.BusinessOperations.DocumentManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.BusinessOperations.DocumentManagement.Permissions
{
    /// <summary>
    /// Defines permissions for document management
    /// </summary>
    public class DocumentManagementPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var documentManagementGroup = context.AddGroup(
                DocumentManagementPermissions.GroupName,
                L("Permission:DocumentManagement"));

            var documentsPermission = documentManagementGroup.AddPermission(
                DocumentManagementPermissions.Documents.Default,
                L("Permission:Documents"));

            documentsPermission.AddChild(
                DocumentManagementPermissions.Documents.Create,
                L("Permission:Documents.Create"));

            documentsPermission.AddChild(
                DocumentManagementPermissions.Documents.Edit,
                L("Permission:Documents.Edit"));

            documentsPermission.AddChild(
                DocumentManagementPermissions.Documents.Delete,
                L("Permission:Documents.Delete"));

            documentsPermission.AddChild(
                DocumentManagementPermissions.Documents.View,
                L("Permission:Documents.View"));

            documentsPermission.AddChild(
                DocumentManagementPermissions.Documents.ManageAccess,
                L("Permission:Documents.ManageAccess"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<DocumentManagementResource>(name);
        }
    }
}
