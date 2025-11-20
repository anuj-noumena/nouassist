using NouAssist.RequirementsManagement.Elicitation.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.RequirementsManagement.Elicitation.Permissions
{
    public class ElicitationPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var elicitationGroup = context.AddGroup(ElicitationPermissions.GroupName, L("Permission:Elicitation"));

            // StakeholderProfile permissions
            var stakeholderProfilesPermission = elicitationGroup.AddPermission(
                ElicitationPermissions.StakeholderProfiles.Default, L("Permission:StakeholderProfiles"));

            stakeholderProfilesPermission.AddChild(
                ElicitationPermissions.StakeholderProfiles.Create, L("Permission:StakeholderProfiles.Create"));
            stakeholderProfilesPermission.AddChild(
                ElicitationPermissions.StakeholderProfiles.Update, L("Permission:StakeholderProfiles.Update"));
            stakeholderProfilesPermission.AddChild(
                ElicitationPermissions.StakeholderProfiles.Delete, L("Permission:StakeholderProfiles.Delete"));
            stakeholderProfilesPermission.AddChild(
                ElicitationPermissions.StakeholderProfiles.View, L("Permission:StakeholderProfiles.View"));
            stakeholderProfilesPermission.AddChild(
                ElicitationPermissions.StakeholderProfiles.ManageAll, L("Permission:StakeholderProfiles.ManageAll"));

            // ElicitationSession permissions
            var elicitationSessionsPermission = elicitationGroup.AddPermission(
                ElicitationPermissions.ElicitationSessions.Default, L("Permission:ElicitationSessions"));

            elicitationSessionsPermission.AddChild(
                ElicitationPermissions.ElicitationSessions.Create, L("Permission:ElicitationSessions.Create"));
            elicitationSessionsPermission.AddChild(
                ElicitationPermissions.ElicitationSessions.Update, L("Permission:ElicitationSessions.Update"));
            elicitationSessionsPermission.AddChild(
                ElicitationPermissions.ElicitationSessions.Delete, L("Permission:ElicitationSessions.Delete"));
            elicitationSessionsPermission.AddChild(
                ElicitationPermissions.ElicitationSessions.View, L("Permission:ElicitationSessions.View"));
            elicitationSessionsPermission.AddChild(
                ElicitationPermissions.ElicitationSessions.Manage, L("Permission:ElicitationSessions.Manage"));
            elicitationSessionsPermission.AddChild(
                ElicitationPermissions.ElicitationSessions.Schedule, L("Permission:ElicitationSessions.Schedule"));
            elicitationSessionsPermission.AddChild(
                ElicitationPermissions.ElicitationSessions.Cancel, L("Permission:ElicitationSessions.Cancel"));

            // ElicitationArtifact permissions
            var elicitationArtifactsPermission = elicitationGroup.AddPermission(
                ElicitationPermissions.ElicitationArtifacts.Default, L("Permission:ElicitationArtifacts"));

            elicitationArtifactsPermission.AddChild(
                ElicitationPermissions.ElicitationArtifacts.Create, L("Permission:ElicitationArtifacts.Create"));
            elicitationArtifactsPermission.AddChild(
                ElicitationPermissions.ElicitationArtifacts.Update, L("Permission:ElicitationArtifacts.Update"));
            elicitationArtifactsPermission.AddChild(
                ElicitationPermissions.ElicitationArtifacts.Delete, L("Permission:ElicitationArtifacts.Delete"));
            elicitationArtifactsPermission.AddChild(
                ElicitationPermissions.ElicitationArtifacts.View, L("Permission:ElicitationArtifacts.View"));
            elicitationArtifactsPermission.AddChild(
                ElicitationPermissions.ElicitationArtifacts.Approve, L("Permission:ElicitationArtifacts.Approve"));
            elicitationArtifactsPermission.AddChild(
                ElicitationPermissions.ElicitationArtifacts.Reject, L("Permission:ElicitationArtifacts.Reject"));

            // Survey permissions
            var surveysPermission = elicitationGroup.AddPermission(
                ElicitationPermissions.Surveys.Default, L("Permission:Surveys"));

            surveysPermission.AddChild(
                ElicitationPermissions.Surveys.Create, L("Permission:Surveys.Create"));
            surveysPermission.AddChild(
                ElicitationPermissions.Surveys.Deploy, L("Permission:Surveys.Deploy"));
            surveysPermission.AddChild(
                ElicitationPermissions.Surveys.ViewResults, L("Permission:Surveys.ViewResults"));
            surveysPermission.AddChild(
                ElicitationPermissions.Surveys.Respond, L("Permission:Surveys.Respond"));

            // Interview permissions
            var interviewsPermission = elicitationGroup.AddPermission(
                ElicitationPermissions.Interviews.Default, L("Permission:Interviews"));

            interviewsPermission.AddChild(
                ElicitationPermissions.Interviews.Schedule, L("Permission:Interviews.Schedule"));
            interviewsPermission.AddChild(
                ElicitationPermissions.Interviews.Conduct, L("Permission:Interviews.Conduct"));
            interviewsPermission.AddChild(
                ElicitationPermissions.Interviews.ViewRecords, L("Permission:Interviews.ViewRecords"));

            // BrainstormingIdea permissions
            var brainstormingIdeasPermission = elicitationGroup.AddPermission(
                ElicitationPermissions.BrainstormingIdeas.Default, L("Permission:BrainstormingIdeas"));

            brainstormingIdeasPermission.AddChild(
                ElicitationPermissions.BrainstormingIdeas.Create, L("Permission:BrainstormingIdeas.Create"));
            brainstormingIdeasPermission.AddChild(
                ElicitationPermissions.BrainstormingIdeas.Update, L("Permission:BrainstormingIdeas.Update"));
            brainstormingIdeasPermission.AddChild(
                ElicitationPermissions.BrainstormingIdeas.Delete, L("Permission:BrainstormingIdeas.Delete"));
            brainstormingIdeasPermission.AddChild(
                ElicitationPermissions.BrainstormingIdeas.View, L("Permission:BrainstormingIdeas.View"));
            brainstormingIdeasPermission.AddChild(
                ElicitationPermissions.BrainstormingIdeas.Select, L("Permission:BrainstormingIdeas.Select"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<ElicitationResource>(name);
        }
    }
}
