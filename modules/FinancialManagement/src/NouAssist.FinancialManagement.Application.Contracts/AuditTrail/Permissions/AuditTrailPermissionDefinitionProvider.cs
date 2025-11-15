using NouAssist.AuditTrail.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.AuditTrail.Permissions
{
    public class AuditTrailPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var auditTrailGroup = context.AddGroup(
                AuditTrailPermissions.GroupName,
                L("Permission:AuditTrail"));

            var auditEntriesPermission = auditTrailGroup.AddPermission(
                AuditTrailPermissions.AuditEntries.Default,
                L("Permission:AuditEntries"));

            auditEntriesPermission.AddChild(
                AuditTrailPermissions.AuditEntries.View,
                L("Permission:AuditEntries.View"));

            auditEntriesPermission.AddChild(
                AuditTrailPermissions.AuditEntries.ViewSensitive,
                L("Permission:AuditEntries.ViewSensitive"));

            auditEntriesPermission.AddChild(
                AuditTrailPermissions.AuditEntries.GenerateReports,
                L("Permission:AuditEntries.GenerateReports"));

            auditEntriesPermission.AddChild(
                AuditTrailPermissions.AuditEntries.Archive,
                L("Permission:AuditEntries.Archive"));

            auditEntriesPermission.AddChild(
                AuditTrailPermissions.AuditEntries.Delete,
                L("Permission:AuditEntries.Delete"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<AuditTrailResource>(name);
        }
    }
}
