using NouAssist.BusinessOperations.FreelanceOperations.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.BusinessOperations.FreelanceOperations.Permissions
{
    public class FreelanceOperationsPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var freelanceOperationsGroup = context.AddGroup(
                FreelanceOperationsPermissions.GroupName,
                L("Permission:FreelanceOperations"));

            // Project permissions
            var projectPermission = freelanceOperationsGroup.AddPermission(
                FreelanceOperationsPermissions.Project.Default,
                L("Permission:Projects"));
            
            projectPermission.AddChild(
                FreelanceOperationsPermissions.Project.Create,
                L("Permission:Projects.Create"));
                
            projectPermission.AddChild(
                FreelanceOperationsPermissions.Project.Update,
                L("Permission:Projects.Update"));
                
            projectPermission.AddChild(
                FreelanceOperationsPermissions.Project.Delete,
                L("Permission:Projects.Delete"));
                
            projectPermission.AddChild(
                FreelanceOperationsPermissions.Project.View,
                L("Permission:Projects.View"));
                
            projectPermission.AddChild(
                FreelanceOperationsPermissions.Project.ManageTimeEntries,
                L("Permission:Projects.ManageTimeEntries"));
                
            projectPermission.AddChild(
                FreelanceOperationsPermissions.Project.ManageMilestones,
                L("Permission:Projects.ManageMilestones"));

            // Client permissions
            var clientPermission = freelanceOperationsGroup.AddPermission(
                FreelanceOperationsPermissions.Client.Default,
                L("Permission:Clients"));
                
            clientPermission.AddChild(
                FreelanceOperationsPermissions.Client.Create,
                L("Permission:Clients.Create"));
                
            clientPermission.AddChild(
                FreelanceOperationsPermissions.Client.Update,
                L("Permission:Clients.Update"));
                
            clientPermission.AddChild(
                FreelanceOperationsPermissions.Client.Delete,
                L("Permission:Clients.Delete"));
                
            clientPermission.AddChild(
                FreelanceOperationsPermissions.Client.View,
                L("Permission:Clients.View"));
                
            clientPermission.AddChild(
                FreelanceOperationsPermissions.Client.ManageCommunications,
                L("Permission:Clients.ManageCommunications"));

            // Proposal permissions
            var proposalPermission = freelanceOperationsGroup.AddPermission(
                FreelanceOperationsPermissions.Proposal.Default,
                L("Permission:Proposals"));
                
            proposalPermission.AddChild(
                FreelanceOperationsPermissions.Proposal.Create,
                L("Permission:Proposals.Create"));
                
            proposalPermission.AddChild(
                FreelanceOperationsPermissions.Proposal.Update,
                L("Permission:Proposals.Update"));
                
            proposalPermission.AddChild(
                FreelanceOperationsPermissions.Proposal.Delete,
                L("Permission:Proposals.Delete"));
                
            proposalPermission.AddChild(
                FreelanceOperationsPermissions.Proposal.View,
                L("Permission:Proposals.View"));
                
            proposalPermission.AddChild(
                FreelanceOperationsPermissions.Proposal.Send,
                L("Permission:Proposals.Send"));
                
            proposalPermission.AddChild(
                FreelanceOperationsPermissions.Proposal.ManageVersions,
                L("Permission:Proposals.ManageVersions"));

            // Upwork permissions
            var upworkPermission = freelanceOperationsGroup.AddPermission(
                FreelanceOperationsPermissions.Upwork.Default,
                L("Permission:Upwork"));
                
            upworkPermission.AddChild(
                FreelanceOperationsPermissions.Upwork.ManageCredentials,
                L("Permission:Upwork.ManageCredentials"));
                
            upworkPermission.AddChild(
                FreelanceOperationsPermissions.Upwork.ManageEngagements,
                L("Permission:Upwork.ManageEngagements"));
                
            upworkPermission.AddChild(
                FreelanceOperationsPermissions.Upwork.ImportJobs,
                L("Permission:Upwork.ImportJobs"));
                
            upworkPermission.AddChild(
                FreelanceOperationsPermissions.Upwork.View,
                L("Permission:Upwork.View"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<FreelanceOperationsResource>(name);
        }
    }
}
