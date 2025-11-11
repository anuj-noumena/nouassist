using NouAssist.WorkManagement.TaskManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.WorkManagement.TaskManagement.Permissions
{
    public class TasksPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var tasksGroup = context.AddGroup(
                TasksPermissions.GroupName,
                L("Permission:Tasks"));

            // Task Blueprints
            var tasksPermission = tasksGroup.AddPermission(
                TasksPermissions.Tasks.Default,
                L("Permission:Tasks.Tasks"));
            
            tasksPermission.AddChild(
                TasksPermissions.Tasks.Create,
                L("Permission:Tasks.Tasks.Create"));
            
            tasksPermission.AddChild(
                TasksPermissions.Tasks.Update,
                L("Permission:Tasks.Tasks.Update"));
            
            tasksPermission.AddChild(
                TasksPermissions.Tasks.Delete,
                L("Permission:Tasks.Tasks.Delete"));
            
            tasksPermission.AddChild(
                TasksPermissions.Tasks.ManageActionSteps,
                L("Permission:Tasks.Tasks.ManageActionSteps"));
            
            tasksPermission.AddChild(
                TasksPermissions.Tasks.ManageCriteria,
                L("Permission:Tasks.Tasks.ManageCriteria"));

            // Task Instances
            var instancesPermission = tasksGroup.AddPermission(
                TasksPermissions.TaskInstances.Default,
                L("Permission:Tasks.TaskInstances"));
            
            instancesPermission.AddChild(
                TasksPermissions.TaskInstances.Create,
                L("Permission:Tasks.TaskInstances.Create"));
            
            instancesPermission.AddChild(
                TasksPermissions.TaskInstances.Update,
                L("Permission:Tasks.TaskInstances.Update"));
            
            instancesPermission.AddChild(
                TasksPermissions.TaskInstances.Delete,
                L("Permission:Tasks.TaskInstances.Delete"));
            
            instancesPermission.AddChild(
                TasksPermissions.TaskInstances.Start,
                L("Permission:Tasks.TaskInstances.Start"));
            
            instancesPermission.AddChild(
                TasksPermissions.TaskInstances.Complete,
                L("Permission:Tasks.TaskInstances.Complete"));
            
            instancesPermission.AddChild(
                TasksPermissions.TaskInstances.Delegate,
                L("Permission:Tasks.TaskInstances.Delegate"));
            
            instancesPermission.AddChild(
                TasksPermissions.TaskInstances.Block,
                L("Permission:Tasks.TaskInstances.Block"));
            
            instancesPermission.AddChild(
                TasksPermissions.TaskInstances.UpdateProgress,
                L("Permission:Tasks.TaskInstances.UpdateProgress"));

            // Task Types
            var typesPermission = tasksGroup.AddPermission(
                TasksPermissions.TaskTypes.Default,
                L("Permission:Tasks.TaskTypes"));
            
            typesPermission.AddChild(
                TasksPermissions.TaskTypes.Create,
                L("Permission:Tasks.TaskTypes.Create"));
            
            typesPermission.AddChild(
                TasksPermissions.TaskTypes.Update,
                L("Permission:Tasks.TaskTypes.Update"));
            
            typesPermission.AddChild(
                TasksPermissions.TaskTypes.Delete,
                L("Permission:Tasks.TaskTypes.Delete"));
            
            typesPermission.AddChild(
                TasksPermissions.TaskTypes.ManageCustomFields,
                L("Permission:Tasks.TaskTypes.ManageCustomFields"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<TasksResource>(name);
        }
    }
}
