using NouAssist.RequirementsManagement.QualityAssurance.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.RequirementsManagement.QualityAssurance.Permissions
{
    public class QAPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            // Define a top-level group for the module
            var qaGroup = context.AddGroup(QAPermissions.GroupName, L("Permission:QA"));

            // Define permissions for TestPlan management
            var testPlanPermissionGroup = qaGroup.AddPermission(
                QAPermissions.TestPlan.Default, L("Permission:TestPlan"));

            testPlanPermissionGroup.AddChild(
                QAPermissions.TestPlan.Create, L("Permission:TestPlan.Create"));
            testPlanPermissionGroup.AddChild(
                QAPermissions.TestPlan.Update, L("Permission:TestPlan.Update"));
            testPlanPermissionGroup.AddChild(
                QAPermissions.TestPlan.Delete, L("Permission:TestPlan.Delete"));
            testPlanPermissionGroup.AddChild(
                QAPermissions.TestPlan.View, L("Permission:TestPlan.View"));
            testPlanPermissionGroup.AddChild(
                QAPermissions.TestPlan.Activate, L("Permission:TestPlan.Activate"));
            testPlanPermissionGroup.AddChild(
                QAPermissions.TestPlan.Complete, L("Permission:TestPlan.Complete"));
            testPlanPermissionGroup.AddChild(
                QAPermissions.TestPlan.Archive, L("Permission:TestPlan.Archive"));

            // Define permissions for TestCycle management
            var testCyclePermissionGroup = qaGroup.AddPermission(
                QAPermissions.TestCycle.Default, L("Permission:TestCycle"));

            testCyclePermissionGroup.AddChild(
                QAPermissions.TestCycle.Create, L("Permission:TestCycle.Create"));
            testCyclePermissionGroup.AddChild(
                QAPermissions.TestCycle.Update, L("Permission:TestCycle.Update"));
            testCyclePermissionGroup.AddChild(
                QAPermissions.TestCycle.Delete, L("Permission:TestCycle.Delete"));
            testCyclePermissionGroup.AddChild(
                QAPermissions.TestCycle.View, L("Permission:TestCycle.View"));
            testCyclePermissionGroup.AddChild(
                QAPermissions.TestCycle.Start, L("Permission:TestCycle.Start"));
            testCyclePermissionGroup.AddChild(
                QAPermissions.TestCycle.Complete, L("Permission:TestCycle.Complete"));
            testCyclePermissionGroup.AddChild(
                QAPermissions.TestCycle.ManageTestRuns, L("Permission:TestCycle.ManageTestRuns"));
            testCyclePermissionGroup.AddChild(
                QAPermissions.TestCycle.RecordResults, L("Permission:TestCycle.RecordResults"));

            // Define permissions for Defect management
            var defectPermissionGroup = qaGroup.AddPermission(
                QAPermissions.Defect.Default, L("Permission:Defect"));

            defectPermissionGroup.AddChild(
                QAPermissions.Defect.Create, L("Permission:Defect.Create"));
            defectPermissionGroup.AddChild(
                QAPermissions.Defect.Update, L("Permission:Defect.Update"));
            defectPermissionGroup.AddChild(
                QAPermissions.Defect.Delete, L("Permission:Defect.Delete"));
            defectPermissionGroup.AddChild(
                QAPermissions.Defect.View, L("Permission:Defect.View"));
            defectPermissionGroup.AddChild(
                QAPermissions.Defect.Assign, L("Permission:Defect.Assign"));
            defectPermissionGroup.AddChild(
                QAPermissions.Defect.ChangeStatus, L("Permission:Defect.ChangeStatus"));
            defectPermissionGroup.AddChild(
                QAPermissions.Defect.VerifyFix, L("Permission:Defect.VerifyFix"));
            defectPermissionGroup.AddChild(
                QAPermissions.Defect.Comment, L("Permission:Defect.Comment"));

            // Define permissions for Quality Reporting
            var reportPermissionGroup = qaGroup.AddPermission(
                QAPermissions.Report.Default, L("Permission:Report"));

            reportPermissionGroup.AddChild(
                QAPermissions.Report.View, L("Permission:Report.View"));
            reportPermissionGroup.AddChild(
                QAPermissions.Report.Export, L("Permission:Report.Export"));
            reportPermissionGroup.AddChild(
                QAPermissions.Report.ManageMetrics, L("Permission:Report.ManageMetrics"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<QAResource>(name);
        }
    }
}
