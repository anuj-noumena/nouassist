using NouAssist.RequirementsManagement.Specification.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.RequirementsManagement.Specification.Permissions
{
    public class RequirementSpecificationManagementPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var requirementSpecGroup = context.AddGroup(
                RequirementSpecificationManagementPermissions.GroupName,
                L("Permission:RequirementSpecificationManagement"));

            // Requirements permissions
            var requirementsPermission = requirementSpecGroup.AddPermission(
                RequirementSpecificationManagementPermissions.Requirements.Default,
                L("Permission:Requirements"));

            requirementsPermission.AddChild(
                RequirementSpecificationManagementPermissions.Requirements.View,
                L("Permission:Requirements.View"));

            requirementsPermission.AddChild(
                RequirementSpecificationManagementPermissions.Requirements.Create,
                L("Permission:Requirements.Create"));

            requirementsPermission.AddChild(
                RequirementSpecificationManagementPermissions.Requirements.Update,
                L("Permission:Requirements.Update"));

            requirementsPermission.AddChild(
                RequirementSpecificationManagementPermissions.Requirements.Delete,
                L("Permission:Requirements.Delete"));

            requirementsPermission.AddChild(
                RequirementSpecificationManagementPermissions.Requirements.ChangeStatus,
                L("Permission:Requirements.ChangeStatus"));

            requirementsPermission.AddChild(
                RequirementSpecificationManagementPermissions.Requirements.ManageAcceptanceCriteria,
                L("Permission:Requirements.ManageAcceptanceCriteria"));

            requirementsPermission.AddChild(
                RequirementSpecificationManagementPermissions.Requirements.ManageBusinessRules,
                L("Permission:Requirements.ManageBusinessRules"));

            requirementsPermission.AddChild(
                RequirementSpecificationManagementPermissions.Requirements.ManageConstraints,
                L("Permission:Requirements.ManageConstraints"));

            requirementsPermission.AddChild(
                RequirementSpecificationManagementPermissions.Requirements.ManageDataRequirements,
                L("Permission:Requirements.ManageDataRequirements"));

            requirementsPermission.AddChild(
                RequirementSpecificationManagementPermissions.Requirements.ManageInterfaces,
                L("Permission:Requirements.ManageInterfaces"));

            requirementsPermission.AddChild(
                RequirementSpecificationManagementPermissions.Requirements.LinkToObjectives,
                L("Permission:Requirements.LinkToObjectives"));

            requirementsPermission.AddChild(
                RequirementSpecificationManagementPermissions.Requirements.SetScope,
                L("Permission:Requirements.SetScope"));

            // Specification Structures permissions
            var structuresPermission = requirementSpecGroup.AddPermission(
                RequirementSpecificationManagementPermissions.SpecificationStructures.Default,
                L("Permission:SpecificationStructures"));

            structuresPermission.AddChild(
                RequirementSpecificationManagementPermissions.SpecificationStructures.View,
                L("Permission:SpecificationStructures.View"));

            structuresPermission.AddChild(
                RequirementSpecificationManagementPermissions.SpecificationStructures.Create,
                L("Permission:SpecificationStructures.Create"));

            structuresPermission.AddChild(
                RequirementSpecificationManagementPermissions.SpecificationStructures.Update,
                L("Permission:SpecificationStructures.Update"));

            structuresPermission.AddChild(
                RequirementSpecificationManagementPermissions.SpecificationStructures.Delete,
                L("Permission:SpecificationStructures.Delete"));

            structuresPermission.AddChild(
                RequirementSpecificationManagementPermissions.SpecificationStructures.ManageSections,
                L("Permission:SpecificationStructures.ManageSections"));

            structuresPermission.AddChild(
                RequirementSpecificationManagementPermissions.SpecificationStructures.GenerateDocuments,
                L("Permission:SpecificationStructures.GenerateDocuments"));

            // Reporting permissions
            var reportingPermission = requirementSpecGroup.AddPermission(
                RequirementSpecificationManagementPermissions.Reporting.Default,
                L("Permission:Reporting"));

            reportingPermission.AddChild(
                RequirementSpecificationManagementPermissions.Reporting.ViewReports,
                L("Permission:Reporting.ViewReports"));

            reportingPermission.AddChild(
                RequirementSpecificationManagementPermissions.Reporting.ExportData,
                L("Permission:Reporting.ExportData"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<RequirementSpecificationManagementResource>(name);
        }
    }
}
