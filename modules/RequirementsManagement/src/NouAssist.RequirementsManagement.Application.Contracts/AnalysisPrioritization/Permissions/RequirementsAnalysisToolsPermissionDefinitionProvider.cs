using NouAssist.RequirementsManagement.AnalysisPrioritization.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.Permissions
{
    /// <summary>
    /// Defines permissions for the Requirements Analysis Tools module
    /// </summary>
    public class RequirementsAnalysisToolsPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var moduleGroup = context.AddGroup(
                RequirementsAnalysisToolsPermissions.GroupName,
                L("Permission:RequirementsAnalysisTools"));

            DefineAnalyzedRequirementsPermissions(moduleGroup);
            DefineGlossaryTermsPermissions(moduleGroup);
            DefinePrioritizationActivitiesPermissions(moduleGroup);
        }

        private static void DefineAnalyzedRequirementsPermissions(PermissionGroupDefinition moduleGroup)
        {
            var analyzedRequirementsPermission = moduleGroup.AddPermission(
                RequirementsAnalysisToolsPermissions.AnalyzedRequirements.Default,
                L("Permission:AnalyzedRequirements"));

            analyzedRequirementsPermission.AddChild(
                RequirementsAnalysisToolsPermissions.AnalyzedRequirements.Create,
                L("Permission:AnalyzedRequirements.Create"));

            analyzedRequirementsPermission.AddChild(
                RequirementsAnalysisToolsPermissions.AnalyzedRequirements.Update,
                L("Permission:AnalyzedRequirements.Update"));

            analyzedRequirementsPermission.AddChild(
                RequirementsAnalysisToolsPermissions.AnalyzedRequirements.Delete,
                L("Permission:AnalyzedRequirements.Delete"));

            analyzedRequirementsPermission.AddChild(
                RequirementsAnalysisToolsPermissions.AnalyzedRequirements.View,
                L("Permission:AnalyzedRequirements.View"));

            analyzedRequirementsPermission.AddChild(
                RequirementsAnalysisToolsPermissions.AnalyzedRequirements.ManageAttributes,
                L("Permission:AnalyzedRequirements.ManageAttributes"));

            analyzedRequirementsPermission.AddChild(
                RequirementsAnalysisToolsPermissions.AnalyzedRequirements.ManageRelationships,
                L("Permission:AnalyzedRequirements.ManageRelationships"));

            analyzedRequirementsPermission.AddChild(
                RequirementsAnalysisToolsPermissions.AnalyzedRequirements.ManageImpactAssessment,
                L("Permission:AnalyzedRequirements.ManageImpactAssessment"));
        }

        private static void DefineGlossaryTermsPermissions(PermissionGroupDefinition moduleGroup)
        {
            var glossaryTermsPermission = moduleGroup.AddPermission(
                RequirementsAnalysisToolsPermissions.GlossaryTerms.Default,
                L("Permission:GlossaryTerms"));

            glossaryTermsPermission.AddChild(
                RequirementsAnalysisToolsPermissions.GlossaryTerms.Create,
                L("Permission:GlossaryTerms.Create"));

            glossaryTermsPermission.AddChild(
                RequirementsAnalysisToolsPermissions.GlossaryTerms.Update,
                L("Permission:GlossaryTerms.Update"));

            glossaryTermsPermission.AddChild(
                RequirementsAnalysisToolsPermissions.GlossaryTerms.Delete,
                L("Permission:GlossaryTerms.Delete"));

            glossaryTermsPermission.AddChild(
                RequirementsAnalysisToolsPermissions.GlossaryTerms.View,
                L("Permission:GlossaryTerms.View"));

            glossaryTermsPermission.AddChild(
                RequirementsAnalysisToolsPermissions.GlossaryTerms.Approve,
                L("Permission:GlossaryTerms.Approve"));

            glossaryTermsPermission.AddChild(
                RequirementsAnalysisToolsPermissions.GlossaryTerms.ManageRelatedTerms,
                L("Permission:GlossaryTerms.ManageRelatedTerms"));
        }

        private static void DefinePrioritizationActivitiesPermissions(PermissionGroupDefinition moduleGroup)
        {
            var prioritizationActivitiesPermission = moduleGroup.AddPermission(
                RequirementsAnalysisToolsPermissions.PrioritizationActivities.Default,
                L("Permission:PrioritizationActivities"));

            prioritizationActivitiesPermission.AddChild(
                RequirementsAnalysisToolsPermissions.PrioritizationActivities.Create,
                L("Permission:PrioritizationActivities.Create"));

            prioritizationActivitiesPermission.AddChild(
                RequirementsAnalysisToolsPermissions.PrioritizationActivities.Update,
                L("Permission:PrioritizationActivities.Update"));

            prioritizationActivitiesPermission.AddChild(
                RequirementsAnalysisToolsPermissions.PrioritizationActivities.Delete,
                L("Permission:PrioritizationActivities.Delete"));

            prioritizationActivitiesPermission.AddChild(
                RequirementsAnalysisToolsPermissions.PrioritizationActivities.View,
                L("Permission:PrioritizationActivities.View"));

            prioritizationActivitiesPermission.AddChild(
                RequirementsAnalysisToolsPermissions.PrioritizationActivities.Manage,
                L("Permission:PrioritizationActivities.Manage"));

            prioritizationActivitiesPermission.AddChild(
                RequirementsAnalysisToolsPermissions.PrioritizationActivities.Vote,
                L("Permission:PrioritizationActivities.Vote"));

            prioritizationActivitiesPermission.AddChild(
                RequirementsAnalysisToolsPermissions.PrioritizationActivities.ViewResults,
                L("Permission:PrioritizationActivities.ViewResults"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<RequirementsAnalysisToolsResource>(name);
        }
    }
}
