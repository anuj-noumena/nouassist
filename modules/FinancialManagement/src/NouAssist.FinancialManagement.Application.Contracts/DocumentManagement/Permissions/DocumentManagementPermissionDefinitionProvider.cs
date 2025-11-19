using NouAssist.FinancialManagement.DocumentManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.FinancialManagement.DocumentManagement.Permissions
{
    public class DocumentManagementPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var documentGroup = context.AddGroup(
                DocumentManagementPermissions.GroupName,
                L("Permission:DocumentManagement"));

            var documentsPermission = documentGroup.AddPermission(
                DocumentManagementPermissions.Documents.Default,
                L("Permission:Documents"));

            documentsPermission.AddChild(
                DocumentManagementPermissions.Documents.View,
                L("Permission:Documents.View"));

            documentsPermission.AddChild(
                DocumentManagementPermissions.Documents.Upload,
                L("Permission:Documents.Upload"));

            documentsPermission.AddChild(
                DocumentManagementPermissions.Documents.Download,
                L("Permission:Documents.Download"));

            documentsPermission.AddChild(
                DocumentManagementPermissions.Documents.Update,
                L("Permission:Documents.Update"));

            documentsPermission.AddChild(
                DocumentManagementPermissions.Documents.Delete,
                L("Permission:Documents.Delete"));

            documentsPermission.AddChild(
                DocumentManagementPermissions.Documents.Archive,
                L("Permission:Documents.Archive"));

            documentsPermission.AddChild(
                DocumentManagementPermissions.Documents.ProcessOCR,
                L("Permission:Documents.ProcessOCR"));

            var foldersPermission = documentGroup.AddPermission(
                DocumentManagementPermissions.Folders.Default,
                L("Permission:Folders"));

            foldersPermission.AddChild(
                DocumentManagementPermissions.Folders.View,
                L("Permission:Folders.View"));

            foldersPermission.AddChild(
                DocumentManagementPermissions.Folders.Create,
                L("Permission:Folders.Create"));

            foldersPermission.AddChild(
                DocumentManagementPermissions.Folders.Update,
                L("Permission:Folders.Update"));

            foldersPermission.AddChild(
                DocumentManagementPermissions.Folders.Delete,
                L("Permission:Folders.Delete"));

            foldersPermission.AddChild(
                DocumentManagementPermissions.Folders.Manage,
                L("Permission:Folders.Manage"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<DocumentManagementResource>(name);
        }
    }
}
