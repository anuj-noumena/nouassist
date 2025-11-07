using NouAssist.CoreProductivity.ProjectManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.CoreProductivity.ProjectManagement.Permissions
{
    public class ProjectsPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var projectsGroup = context.AddGroup(
                ProjectsPermissions.GroupName,
                L("Permission:Projects"));

            // Projects Permissions
            var projectsPermission = projectsGroup.AddPermission(
                ProjectsPermissions.Projects.Default,
                L("Permission:Projects.Projects"));
            
            projectsPermission.AddChild(
                ProjectsPermissions.Projects.Create,
                L("Permission:Projects.Projects.Create"));
            
            projectsPermission.AddChild(
                ProjectsPermissions.Projects.Update,
                L("Permission:Projects.Projects.Update"));
            
            projectsPermission.AddChild(
                ProjectsPermissions.Projects.Delete,
                L("Permission:Projects.Projects.Delete"));
            
            projectsPermission.AddChild(
                ProjectsPermissions.Projects.Complete,
                L("Permission:Projects.Projects.Complete"));
            
            projectsPermission.AddChild(
                ProjectsPermissions.Projects.Cancel,
                L("Permission:Projects.Projects.Cancel"));
            
            projectsPermission.AddChild(
                ProjectsPermissions.Projects.Archive,
                L("Permission:Projects.Projects.Archive"));

            // Sprints Permissions
            var sprintsPermission = projectsGroup.AddPermission(
                ProjectsPermissions.Sprints.Default,
                L("Permission:Projects.Sprints"));
            
            sprintsPermission.AddChild(
                ProjectsPermissions.Sprints.Create,
                L("Permission:Projects.Sprints.Create"));
            
            sprintsPermission.AddChild(
                ProjectsPermissions.Sprints.Update,
                L("Permission:Projects.Sprints.Update"));
            
            sprintsPermission.AddChild(
                ProjectsPermissions.Sprints.Delete,
                L("Permission:Projects.Sprints.Delete"));
            
            sprintsPermission.AddChild(
                ProjectsPermissions.Sprints.Start,
                L("Permission:Projects.Sprints.Start"));
            
            sprintsPermission.AddChild(
                ProjectsPermissions.Sprints.Complete,
                L("Permission:Projects.Sprints.Complete"));

            // Dependencies Permissions
            var dependenciesPermission = projectsGroup.AddPermission(
                ProjectsPermissions.Dependencies.Default,
                L("Permission:Projects.Dependencies"));
            
            dependenciesPermission.AddChild(
                ProjectsPermissions.Dependencies.Create,
                L("Permission:Projects.Dependencies.Create"));
            
            dependenciesPermission.AddChild(
                ProjectsPermissions.Dependencies.Update,
                L("Permission:Projects.Dependencies.Update"));
            
            dependenciesPermission.AddChild(
                ProjectsPermissions.Dependencies.Delete,
                L("Permission:Projects.Dependencies.Delete"));
            
            dependenciesPermission.AddChild(
                ProjectsPermissions.Dependencies.Resolve,
                L("Permission:Projects.Dependencies.Resolve"));

            // Success Criteria Permissions
            var criteriaPermission = projectsGroup.AddPermission(
                ProjectsPermissions.SuccessCriteria.Default,
                L("Permission:Projects.SuccessCriteria"));
            
            criteriaPermission.AddChild(
                ProjectsPermissions.SuccessCriteria.Create,
                L("Permission:Projects.SuccessCriteria.Create"));
            
            criteriaPermission.AddChild(
                ProjectsPermissions.SuccessCriteria.Update,
                L("Permission:Projects.SuccessCriteria.Update"));
            
            criteriaPermission.AddChild(
                ProjectsPermissions.SuccessCriteria.Delete,
                L("Permission:Projects.SuccessCriteria.Delete"));
            
            criteriaPermission.AddChild(
                ProjectsPermissions.SuccessCriteria.MarkAsMet,
                L("Permission:Projects.SuccessCriteria.MarkAsMet"));

            // Events Permissions
            var eventsPermission = projectsGroup.AddPermission(
                ProjectsPermissions.Events.Default,
                L("Permission:Projects.Events"));
            
            eventsPermission.AddChild(
                ProjectsPermissions.Events.Create,
                L("Permission:Projects.Events.Create"));
            
            eventsPermission.AddChild(
                ProjectsPermissions.Events.Update,
                L("Permission:Projects.Events.Update"));
            
            eventsPermission.AddChild(
                ProjectsPermissions.Events.Delete,
                L("Permission:Projects.Events.Delete"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<ProjectManagementResource>(name);
        }
    }
}
