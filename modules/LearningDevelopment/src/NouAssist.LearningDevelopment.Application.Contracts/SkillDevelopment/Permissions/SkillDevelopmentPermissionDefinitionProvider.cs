using NouAssist.SkillDevelopment.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.LearningDevelopment.SkillDevelopment.Permissions
{
    public class SkillDevelopmentPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var skillDevGroup = context.AddGroup(
                SkillDevelopmentPermissions.GroupName,
                L("Permission:SkillDevelopment"));

            var skillsPermission = skillDevGroup.AddPermission(
                SkillDevelopmentPermissions.Skills.Default,
                L("Permission:Skills"));
            skillsPermission.AddChild(SkillDevelopmentPermissions.Skills.Create, L("Permission:Create"));
            skillsPermission.AddChild(SkillDevelopmentPermissions.Skills.Edit, L("Permission:Edit"));
            skillsPermission.AddChild(SkillDevelopmentPermissions.Skills.Delete, L("Permission:Delete"));
            skillsPermission.AddChild(SkillDevelopmentPermissions.Skills.ManageSubSkills, L("Permission:ManageSubSkills"));

            var sessionsPermission = skillDevGroup.AddPermission(
                SkillDevelopmentPermissions.DevelopmentSessions.Default,
                L("Permission:DevelopmentSessions"));
            sessionsPermission.AddChild(SkillDevelopmentPermissions.DevelopmentSessions.Start, L("Permission:Start"));
            sessionsPermission.AddChild(SkillDevelopmentPermissions.DevelopmentSessions.Manage, L("Permission:Manage"));
            sessionsPermission.AddChild(SkillDevelopmentPermissions.DevelopmentSessions.Complete, L("Permission:Complete"));
            sessionsPermission.AddChild(SkillDevelopmentPermissions.DevelopmentSessions.AddPracticeLog, L("Permission:AddPracticeLog"));
            sessionsPermission.AddChild(SkillDevelopmentPermissions.DevelopmentSessions.AddApplication, L("Permission:AddApplication"));

            var logsPermission = skillDevGroup.AddPermission(
                SkillDevelopmentPermissions.SkillLogs.Default,
                L("Permission:SkillLogs"));
            logsPermission.AddChild(SkillDevelopmentPermissions.SkillLogs.Create, L("Permission:Create"));
            logsPermission.AddChild(SkillDevelopmentPermissions.SkillLogs.Edit, L("Permission:Edit"));
            logsPermission.AddChild(SkillDevelopmentPermissions.SkillLogs.Delete, L("Permission:Delete"));

            var treesPermission = skillDevGroup.AddPermission(
                SkillDevelopmentPermissions.SkillTrees.Default,
                L("Permission:SkillTrees"));
            treesPermission.AddChild(SkillDevelopmentPermissions.SkillTrees.View, L("Permission:View"));
            treesPermission.AddChild(SkillDevelopmentPermissions.SkillTrees.Edit, L("Permission:Edit"));

            var resourcesPermission = skillDevGroup.AddPermission(
                SkillDevelopmentPermissions.Resources.Default,
                L("Permission:Resources"));
            resourcesPermission.AddChild(SkillDevelopmentPermissions.Resources.Track, L("Permission:Track"));
            resourcesPermission.AddChild(SkillDevelopmentPermissions.Resources.View, L("Permission:View"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<SkillDevelopmentResource>(name);
        }
    }
}
