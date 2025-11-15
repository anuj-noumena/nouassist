using NouAssist.CreditScoreMonitoring.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.CreditScoreMonitoring.Permissions
{
    public class CreditScoreMonitoringPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var creditScoreGroup = context.AddGroup(
                CreditScoreMonitoringPermissions.GroupName,
                L("Permission:CreditScoreMonitoring"));

            var creditReports = creditScoreGroup.AddPermission(
                CreditScoreMonitoringPermissions.CreditReports.Default,
                L("Permission:CreditReports"));
            creditReports.AddChild(CreditScoreMonitoringPermissions.CreditReports.View, L("Permission:View"));
            creditReports.AddChild(CreditScoreMonitoringPermissions.CreditReports.ViewDetails, L("Permission:ViewDetails"));
            creditReports.AddChild(CreditScoreMonitoringPermissions.CreditReports.Import, L("Permission:Import"));

            var creditDisputes = creditScoreGroup.AddPermission(
                CreditScoreMonitoringPermissions.CreditDisputes.Default,
                L("Permission:CreditDisputes"));
            creditDisputes.AddChild(CreditScoreMonitoringPermissions.CreditDisputes.Create, L("Permission:Create"));
            creditDisputes.AddChild(CreditScoreMonitoringPermissions.CreditDisputes.View, L("Permission:View"));
            creditDisputes.AddChild(CreditScoreMonitoringPermissions.CreditDisputes.Update, L("Permission:Update"));
            creditDisputes.AddChild(CreditScoreMonitoringPermissions.CreditDisputes.Cancel, L("Permission:Cancel"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<CreditScoreMonitoringResource>(name);
        }
    }
}
