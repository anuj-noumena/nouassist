using NouAssist.BusinessOperations.ProjectExecution.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.BusinessOperations.ProjectExecution.Permissions
{
    public class ProjectExecutionPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var projectExecutionGroup = context.AddGroup(
                ProjectExecutionPermissions.GroupName,
                L("Permission:ProjectExecution"));
                
            var projectsPermission = projectExecutionGroup.AddPermission(
                ProjectExecutionPermissions.Projects.Default,
                L("Permission:Projects"));
                
            projectsPermission.AddChild(
                ProjectExecutionPermissions.Projects.Create,
                L("Permission:Projects.Create"));
                
            projectsPermission.AddChild(
                ProjectExecutionPermissions.Projects.Edit,
                L("Permission:Projects.Edit"));
                
            projectsPermission.AddChild(
                ProjectExecutionPermissions.Projects.Delete,
                L("Permission:Projects.Delete"));
                
            projectsPermission.AddChild(
                ProjectExecutionPermissions.Projects.ManageStatus,
                L("Permission:Projects.ManageStatus"));
                
            projectsPermission.AddChild(
                ProjectExecutionPermissions.Projects.ManageMembers,
                L("Permission:Projects.ManageMembers"));
                
            projectsPermission.AddChild(
                ProjectExecutionPermissions.Projects.ManagePhases,
                L("Permission:Projects.ManagePhases"));
                
            projectsPermission.AddChild(
                ProjectExecutionPermissions.Projects.ManageMilestones,
                L("Permission:Projects.ManageMilestones"));
                
            var tasksPermission = projectExecutionGroup.AddPermission(
                ProjectExecutionPermissions.Tasks.Default,
                L("Permission:Tasks"));
                
            tasksPermission.AddChild(
                ProjectExecutionPermissions.Tasks.Create,
                L("Permission:Tasks.Create"));
                
            tasksPermission.AddChild(
                ProjectExecutionPermissions.Tasks.Edit,
                L("Permission:Tasks.Edit"));
                
            tasksPermission.AddChild(
                ProjectExecutionPermissions.Tasks.Delete,
                L("Permission:Tasks.Delete"));
                
            tasksPermission.AddChild(
                ProjectExecutionPermissions.Tasks.ManageStatus,
                L("Permission:Tasks.ManageStatus"));
                
            tasksPermission.AddChild(
                ProjectExecutionPermissions.Tasks.ManageAssignments,
                L("Permission:Tasks.ManageAssignments"));
                
            tasksPermission.AddChild(
                ProjectExecutionPermissions.Tasks.ManageDependencies,
                L("Permission:Tasks.ManageDependencies"));
                
            var timeLogsPermission = projectExecutionGroup.AddPermission(
                ProjectExecutionPermissions.TimeLogs.Default,
                L("Permission:TimeLogs"));
                
            timeLogsPermission.AddChild(
                ProjectExecutionPermissions.TimeLogs.Create,
                L("Permission:TimeLogs.Create"));
                
            timeLogsPermission.AddChild(
                ProjectExecutionPermissions.TimeLogs.Edit,
                L("Permission:TimeLogs.Edit"));
                
            timeLogsPermission.AddChild(
                ProjectExecutionPermissions.TimeLogs.Delete,
                L("Permission:TimeLogs.Delete"));
                
            timeLogsPermission.AddChild(
                ProjectExecutionPermissions.TimeLogs.ViewOthers,
                L("Permission:TimeLogs.ViewOthers"));
                
            var reportsPermission = projectExecutionGroup.AddPermission(
                ProjectExecutionPermissions.Reports.Default,
                L("Permission:Reports"));
                
            reportsPermission.AddChild(
                ProjectExecutionPermissions.Reports.ViewProjectReports,
                L("Permission:Reports.ViewProjectReports"));
                
            reportsPermission.AddChild(
                ProjectExecutionPermissions.Reports.ViewResourceReports,
                L("Permission:Reports.ViewResourceReports"));
        }
        
        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<ProjectExecutionResource>(name);
        }
    }
}
using NouAssist.BusinessOperations.ProjectExecution.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.BusinessOperations.ProjectExecution.Permissions
{
    /// <summary>
    /// Defines permission groups and permissions for the Project Execution module
    /// </summary>
    public class ProjectExecutionPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var moduleGroup = context.AddGroup(
                ProjectExecutionPermissions.GroupName,
                L("Permission:ProjectExecution"));

            // Project permissions
            var projectsPermission = moduleGroup.AddPermission(
                ProjectExecutionPermissions.Projects.Default,
                L("Permission:Projects"));
                
            projectsPermission.AddChild(
                ProjectExecutionPermissions.Projects.Create,
                L("Permission:Projects.Create"));
                
            projectsPermission.AddChild(
                ProjectExecutionPermissions.Projects.Update,
                L("Permission:Projects.Update"));
                
            projectsPermission.AddChild(
                ProjectExecutionPermissions.Projects.Delete,
                L("Permission:Projects.Delete"));
                
            projectsPermission.AddChild(
                ProjectExecutionPermissions.Projects.View,
                L("Permission:Projects.View"));
                
            projectsPermission.AddChild(
                ProjectExecutionPermissions.Projects.ManagePhases,
                L("Permission:Projects.ManagePhases"));
                
            projectsPermission.AddChild(
                ProjectExecutionPermissions.Projects.ManageMilestones,
                L("Permission:Projects.ManageMilestones"));
                
            projectsPermission.AddChild(
                ProjectExecutionPermissions.Projects.ManageMembers,
                L("Permission:Projects.ManageMembers"));

            // Task permissions
            var tasksPermission = moduleGroup.AddPermission(
                ProjectExecutionPermissions.Tasks.Default,
                L("Permission:Tasks"));
                
            tasksPermission.AddChild(
                ProjectExecutionPermissions.Tasks.Create,
                L("Permission:Tasks.Create"));
                
            tasksPermission.AddChild(
                ProjectExecutionPermissions.Tasks.Update,
                L("Permission:Tasks.Update"));
                
            tasksPermission.AddChild(
                ProjectExecutionPermissions.Tasks.Delete,
                L("Permission:Tasks.Delete"));
                
            tasksPermission.AddChild(
                ProjectExecutionPermissions.Tasks.View,
                L("Permission:Tasks.View"));
                
            tasksPermission.AddChild(
                ProjectExecutionPermissions.Tasks.Assign,
                L("Permission:Tasks.Assign"));
                
            tasksPermission.AddChild(
                ProjectExecutionPermissions.Tasks.ManageDependencies,
                L("Permission:Tasks.ManageDependencies"));

            // Time logging permissions
            var timeLogsPermission = moduleGroup.AddPermission(
                ProjectExecutionPermissions.TimeLogs.Default,
                L("Permission:TimeLogs"));
                
            timeLogsPermission.AddChild(
                ProjectExecutionPermissions.TimeLogs.Create,
                L("Permission:TimeLogs.Create"));
                
            timeLogsPermission.AddChild(
                ProjectExecutionPermissions.TimeLogs.Update,
                L("Permission:TimeLogs.Update"));
                
            timeLogsPermission.AddChild(
                ProjectExecutionPermissions.TimeLogs.Delete,
                L("Permission:TimeLogs.Delete"));
                
            timeLogsPermission.AddChild(
                ProjectExecutionPermissions.TimeLogs.View,
                L("Permission:TimeLogs.View"));
                
            timeLogsPermission.AddChild(
                ProjectExecutionPermissions.TimeLogs.ViewOthers,
                L("Permission:TimeLogs.ViewOthers"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<ProjectExecutionResource>(name);
        }
    }
}
