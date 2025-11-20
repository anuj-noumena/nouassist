namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Permissions
{
    /// <summary>
    /// Definition provider for workflow engine permissions
    /// </summary>
    public class WorkflowEnginePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        /// <summary>
        /// Defines the permissions
        /// </summary>
        /// <param name="context">The permission definition context</param>
        public override void Define(IPermissionDefinitionContext context)
        {
            var workflowEngineGroup = context.AddGroup(
                WorkflowEnginePermissions.GroupName,
                L("Permission:WorkflowEngine"));

            var workflowDefinitionsPermission = workflowEngineGroup.AddPermission(
                WorkflowEnginePermissions.WorkflowDefinitions.Default,
                L("Permission:WorkflowDefinitions"));
                
            workflowDefinitionsPermission.AddChild(
                WorkflowEnginePermissions.WorkflowDefinitions.Create,
                L("Permission:WorkflowDefinitions.Create"));
                
            workflowDefinitionsPermission.AddChild(
                WorkflowEnginePermissions.WorkflowDefinitions.Edit,
                L("Permission:WorkflowDefinitions.Edit"));
                
            workflowDefinitionsPermission.AddChild(
                WorkflowEnginePermissions.WorkflowDefinitions.Delete,
                L("Permission:WorkflowDefinitions.Delete"));
                
            workflowDefinitionsPermission.AddChild(
                WorkflowEnginePermissions.WorkflowDefinitions.Publish,
                L("Permission:WorkflowDefinitions.Publish"));
                
            workflowDefinitionsPermission.AddChild(
                WorkflowEnginePermissions.WorkflowDefinitions.Analyze,
                L("Permission:WorkflowDefinitions.Analyze"));

            var workflowTemplatesPermission = workflowEngineGroup.AddPermission(
                WorkflowEnginePermissions.WorkflowTemplates.Default,
                L("Permission:WorkflowTemplates"));
                
            workflowTemplatesPermission.AddChild(
                WorkflowEnginePermissions.WorkflowTemplates.Create,
                L("Permission:WorkflowTemplates.Create"));
                
            workflowTemplatesPermission.AddChild(
                WorkflowEnginePermissions.WorkflowTemplates.Edit,
                L("Permission:WorkflowTemplates.Edit"));
                
            workflowTemplatesPermission.AddChild(
                WorkflowEnginePermissions.WorkflowTemplates.Delete,
                L("Permission:WorkflowTemplates.Delete"));
                
            workflowTemplatesPermission.AddChild(
                WorkflowEnginePermissions.WorkflowTemplates.MakePublic,
                L("Permission:WorkflowTemplates.MakePublic"));

            var workflowInstancesPermission = workflowEngineGroup.AddPermission(
                WorkflowEnginePermissions.WorkflowInstances.Default,
                L("Permission:WorkflowInstances"));
                
            workflowInstancesPermission.AddChild(
                WorkflowEnginePermissions.WorkflowInstances.Create,
                L("Permission:WorkflowInstances.Create"));
                
            workflowInstancesPermission.AddChild(
                WorkflowEnginePermissions.WorkflowInstances.Manage,
                L("Permission:WorkflowInstances.Manage"));
                
            workflowInstancesPermission.AddChild(
                WorkflowEnginePermissions.WorkflowInstances.AssignTasks,
                L("Permission:WorkflowInstances.AssignTasks"));
                
            workflowInstancesPermission.AddChild(
                WorkflowEnginePermissions.WorkflowInstances.CompleteTasks,
                L("Permission:WorkflowInstances.CompleteTasks"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<WorkflowEngineResource>(name);
        }
    }
}
