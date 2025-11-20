using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using NouAssist.RequirementsManagement.ValidationVerification.Localization;

namespace NouAssist.RequirementsManagement.ValidationVerification.Permissions
{
    /// <summary>
    /// Permission definition provider for ValidationVerification module
    /// </summary>
    public class ValidationVerificationPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var validationVerificationGroup = context.AddGroup(
                ValidationVerificationPermissions.GroupName,
                L("Permission:ValidationVerification"));

            // Review Cycles permissions
            var reviewCyclesPermission = validationVerificationGroup.AddPermission(
                ValidationVerificationPermissions.ReviewCycles.Default,
                L("Permission:ReviewCycles"));

            reviewCyclesPermission.AddChild(
                ValidationVerificationPermissions.ReviewCycles.Create,
                L("Permission:ReviewCycles.Create"));

            reviewCyclesPermission.AddChild(
                ValidationVerificationPermissions.ReviewCycles.Update,
                L("Permission:ReviewCycles.Update"));

            reviewCyclesPermission.AddChild(
                ValidationVerificationPermissions.ReviewCycles.Delete,
                L("Permission:ReviewCycles.Delete"));

            reviewCyclesPermission.AddChild(
                ValidationVerificationPermissions.ReviewCycles.Start,
                L("Permission:ReviewCycles.Start"));

            reviewCyclesPermission.AddChild(
                ValidationVerificationPermissions.ReviewCycles.Cancel,
                L("Permission:ReviewCycles.Cancel"));

            reviewCyclesPermission.AddChild(
                ValidationVerificationPermissions.ReviewCycles.SubmitReview,
                L("Permission:ReviewCycles.SubmitReview"));

            reviewCyclesPermission.AddChild(
                ValidationVerificationPermissions.ReviewCycles.ManageReviewers,
                L("Permission:ReviewCycles.ManageReviewers"));

            reviewCyclesPermission.AddChild(
                ValidationVerificationPermissions.ReviewCycles.ViewAll,
                L("Permission:ReviewCycles.ViewAll"));

            // Test Cases permissions
            var testCasesPermission = validationVerificationGroup.AddPermission(
                ValidationVerificationPermissions.TestCases.Default,
                L("Permission:TestCases"));

            testCasesPermission.AddChild(
                ValidationVerificationPermissions.TestCases.Create,
                L("Permission:TestCases.Create"));

            testCasesPermission.AddChild(
                ValidationVerificationPermissions.TestCases.Update,
                L("Permission:TestCases.Update"));

            testCasesPermission.AddChild(
                ValidationVerificationPermissions.TestCases.Delete,
                L("Permission:TestCases.Delete"));

            testCasesPermission.AddChild(
                ValidationVerificationPermissions.TestCases.Execute,
                L("Permission:TestCases.Execute"));

            testCasesPermission.AddChild(
                ValidationVerificationPermissions.TestCases.ManageSteps,
                L("Permission:TestCases.ManageSteps"));

            testCasesPermission.AddChild(
                ValidationVerificationPermissions.TestCases.ViewResults,
                L("Permission:TestCases.ViewResults"));

            testCasesPermission.AddChild(
                ValidationVerificationPermissions.TestCases.ManageLinks,
                L("Permission:TestCases.ManageLinks"));

            // Test Coverage permissions
            var testCoveragePermission = validationVerificationGroup.AddPermission(
                ValidationVerificationPermissions.TestCoverage.Default,
                L("Permission:TestCoverage"));

            testCoveragePermission.AddChild(
                ValidationVerificationPermissions.TestCoverage.Analyze,
                L("Permission:TestCoverage.Analyze"));

            testCoveragePermission.AddChild(
                ValidationVerificationPermissions.TestCoverage.GenerateReports,
                L("Permission:TestCoverage.GenerateReports"));

            testCoveragePermission.AddChild(
                ValidationVerificationPermissions.TestCoverage.ViewReports,
                L("Permission:TestCoverage.ViewReports"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<ValidationVerificationResource>(name);
        }
    }
}
