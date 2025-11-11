using NouAssist.WorkManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NouAssist.WorkManagement.Permissions;

public class WorkManagementPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var workManagementGroup = context.AddGroup(
            WorkManagementPermissions.GroupName,
            L("Permission:WorkManagement"));

        // Approval Permissions
        var workflowsPermission = workManagementGroup.AddPermission(
            WorkManagementPermissions.Approval.Workflows.Default,
            L("Permission:Workflows"));
        workflowsPermission.AddChild(
            WorkManagementPermissions.Approval.Workflows.Create,
            L("Permission:Workflows.Create"));
        workflowsPermission.AddChild(
            WorkManagementPermissions.Approval.Workflows.Update,
            L("Permission:Workflows.Update"));
        workflowsPermission.AddChild(
            WorkManagementPermissions.Approval.Workflows.Delete,
            L("Permission:Workflows.Delete"));
        workflowsPermission.AddChild(
            WorkManagementPermissions.Approval.Workflows.Activate,
            L("Permission:Workflows.Activate"));
        workflowsPermission.AddChild(
            WorkManagementPermissions.Approval.Workflows.Deactivate,
            L("Permission:Workflows.Deactivate"));

        var requestsPermission = workManagementGroup.AddPermission(
            WorkManagementPermissions.Approval.Requests.Default,
            L("Permission:Requests"));
        requestsPermission.AddChild(
            WorkManagementPermissions.Approval.Requests.Create,
            L("Permission:Requests.Create"));
        requestsPermission.AddChild(
            WorkManagementPermissions.Approval.Requests.View,
            L("Permission:Requests.View"));
        requestsPermission.AddChild(
            WorkManagementPermissions.Approval.Requests.Approve,
            L("Permission:Requests.Approve"));
        requestsPermission.AddChild(
            WorkManagementPermissions.Approval.Requests.Reject,
            L("Permission:Requests.Reject"));
        requestsPermission.AddChild(
            WorkManagementPermissions.Approval.Requests.Cancel,
            L("Permission:Requests.Cancel"));

        // GTD Permissions
        var horizonsPermission = workManagementGroup.AddPermission(
            WorkManagementPermissions.GTD.Horizons.Default,
            L("Permission:GTD.Horizons"));
        horizonsPermission.AddChild(WorkManagementPermissions.GTD.Horizons.Create, L("Permission:Create"));
        horizonsPermission.AddChild(WorkManagementPermissions.GTD.Horizons.Update, L("Permission:Update"));
        horizonsPermission.AddChild(WorkManagementPermissions.GTD.Horizons.Delete, L("Permission:Delete"));
        horizonsPermission.AddChild(WorkManagementPermissions.GTD.Horizons.Review, L("Permission:Review"));
        horizonsPermission.AddChild(WorkManagementPermissions.GTD.Horizons.Publish, L("Permission:Publish"));

        var ticklersPermission = workManagementGroup.AddPermission(
            WorkManagementPermissions.GTD.Ticklers.Default,
            L("Permission:GTD.Ticklers"));
        ticklersPermission.AddChild(WorkManagementPermissions.GTD.Ticklers.Create, L("Permission:Create"));
        ticklersPermission.AddChild(WorkManagementPermissions.GTD.Ticklers.Update, L("Permission:Update"));
        ticklersPermission.AddChild(WorkManagementPermissions.GTD.Ticklers.Delete, L("Permission:Delete"));
        ticklersPermission.AddChild(WorkManagementPermissions.GTD.Ticklers.Surface, L("Permission:Surface"));
        ticklersPermission.AddChild(WorkManagementPermissions.GTD.Ticklers.Process, L("Permission:Process"));

        var somedayMaybePermission = workManagementGroup.AddPermission(
            WorkManagementPermissions.GTD.SomedayMaybe.Default,
            L("Permission:GTD.SomedayMaybe"));
        somedayMaybePermission.AddChild(WorkManagementPermissions.GTD.SomedayMaybe.Create, L("Permission:Create"));
        somedayMaybePermission.AddChild(WorkManagementPermissions.GTD.SomedayMaybe.Update, L("Permission:Update"));
        somedayMaybePermission.AddChild(WorkManagementPermissions.GTD.SomedayMaybe.Delete, L("Permission:Delete"));
        somedayMaybePermission.AddChild(WorkManagementPermissions.GTD.SomedayMaybe.Promote, L("Permission:Promote"));
        somedayMaybePermission.AddChild(WorkManagementPermissions.GTD.SomedayMaybe.Review, L("Permission:Review"));

        var waitingForPermission = workManagementGroup.AddPermission(
            WorkManagementPermissions.GTD.WaitingFor.Default,
            L("Permission:GTD.WaitingFor"));
        waitingForPermission.AddChild(WorkManagementPermissions.GTD.WaitingFor.Create, L("Permission:Create"));
        waitingForPermission.AddChild(WorkManagementPermissions.GTD.WaitingFor.Update, L("Permission:Update"));
        waitingForPermission.AddChild(WorkManagementPermissions.GTD.WaitingFor.Delete, L("Permission:Delete"));
        waitingForPermission.AddChild(WorkManagementPermissions.GTD.WaitingFor.Resolve, L("Permission:Resolve"));

        // Project Management Permissions
        var projectsPermission = workManagementGroup.AddPermission(
            WorkManagementPermissions.ProjectManagement.Projects.Default,
            L("Permission:Projects.Projects"));
        projectsPermission.AddChild(
            WorkManagementPermissions.ProjectManagement.Projects.Create,
            L("Permission:Projects.Projects.Create"));
        projectsPermission.AddChild(
            WorkManagementPermissions.ProjectManagement.Projects.Update,
            L("Permission:Projects.Projects.Update"));
        projectsPermission.AddChild(
            WorkManagementPermissions.ProjectManagement.Projects.Delete,
            L("Permission:Projects.Projects.Delete"));
        projectsPermission.AddChild(
            WorkManagementPermissions.ProjectManagement.Projects.Complete,
            L("Permission:Projects.Projects.Complete"));
        projectsPermission.AddChild(
            WorkManagementPermissions.ProjectManagement.Projects.Cancel,
            L("Permission:Projects.Projects.Cancel"));
        projectsPermission.AddChild(
            WorkManagementPermissions.ProjectManagement.Projects.Archive,
            L("Permission:Projects.Projects.Archive"));

        var sprintsPermission = workManagementGroup.AddPermission(
            WorkManagementPermissions.ProjectManagement.Sprints.Default,
            L("Permission:Projects.Sprints"));
        sprintsPermission.AddChild(
            WorkManagementPermissions.ProjectManagement.Sprints.Create,
            L("Permission:Projects.Sprints.Create"));
        sprintsPermission.AddChild(
            WorkManagementPermissions.ProjectManagement.Sprints.Update,
            L("Permission:Projects.Sprints.Update"));
        sprintsPermission.AddChild(
            WorkManagementPermissions.ProjectManagement.Sprints.Delete,
            L("Permission:Projects.Sprints.Delete"));
        sprintsPermission.AddChild(
            WorkManagementPermissions.ProjectManagement.Sprints.Start,
            L("Permission:Projects.Sprints.Start"));
        sprintsPermission.AddChild(
            WorkManagementPermissions.ProjectManagement.Sprints.Complete,
            L("Permission:Projects.Sprints.Complete"));

        var dependenciesPermission = workManagementGroup.AddPermission(
            WorkManagementPermissions.ProjectManagement.Dependencies.Default,
            L("Permission:Projects.Dependencies"));
        dependenciesPermission.AddChild(
            WorkManagementPermissions.ProjectManagement.Dependencies.Create,
            L("Permission:Projects.Dependencies.Create"));
        dependenciesPermission.AddChild(
            WorkManagementPermissions.ProjectManagement.Dependencies.Update,
            L("Permission:Projects.Dependencies.Update"));
        dependenciesPermission.AddChild(
            WorkManagementPermissions.ProjectManagement.Dependencies.Delete,
            L("Permission:Projects.Dependencies.Delete"));
        dependenciesPermission.AddChild(
            WorkManagementPermissions.ProjectManagement.Dependencies.Resolve,
            L("Permission:Projects.Dependencies.Resolve"));

        var criteriaPermission = workManagementGroup.AddPermission(
            WorkManagementPermissions.ProjectManagement.SuccessCriteria.Default,
            L("Permission:Projects.SuccessCriteria"));
        criteriaPermission.AddChild(
            WorkManagementPermissions.ProjectManagement.SuccessCriteria.Create,
            L("Permission:Projects.SuccessCriteria.Create"));
        criteriaPermission.AddChild(
            WorkManagementPermissions.ProjectManagement.SuccessCriteria.Update,
            L("Permission:Projects.SuccessCriteria.Update"));
        criteriaPermission.AddChild(
            WorkManagementPermissions.ProjectManagement.SuccessCriteria.Delete,
            L("Permission:Projects.SuccessCriteria.Delete"));
        criteriaPermission.AddChild(
            WorkManagementPermissions.ProjectManagement.SuccessCriteria.MarkAsMet,
            L("Permission:Projects.SuccessCriteria.MarkAsMet"));

        var eventsPermission = workManagementGroup.AddPermission(
            WorkManagementPermissions.ProjectManagement.Events.Default,
            L("Permission:Projects.Events"));
        eventsPermission.AddChild(
            WorkManagementPermissions.ProjectManagement.Events.Create,
            L("Permission:Projects.Events.Create"));
        eventsPermission.AddChild(
            WorkManagementPermissions.ProjectManagement.Events.Update,
            L("Permission:Projects.Events.Update"));
        eventsPermission.AddChild(
            WorkManagementPermissions.ProjectManagement.Events.Delete,
            L("Permission:Projects.Events.Delete"));

        // Reviews Permissions
        var reviewsPermission = workManagementGroup.AddPermission(
            WorkManagementPermissions.Reviews.ReviewManagement.Default,
            L("Permission:Reviews"));
        reviewsPermission.AddChild(WorkManagementPermissions.Reviews.ReviewManagement.Create, L("Permission:Reviews.Create"));
        reviewsPermission.AddChild(WorkManagementPermissions.Reviews.ReviewManagement.Edit, L("Permission:Reviews.Edit"));
        reviewsPermission.AddChild(WorkManagementPermissions.Reviews.ReviewManagement.Delete, L("Permission:Reviews.Delete"));
        reviewsPermission.AddChild(WorkManagementPermissions.Reviews.ReviewManagement.Complete, L("Permission:Reviews.Complete"));
        reviewsPermission.AddChild(WorkManagementPermissions.Reviews.ReviewManagement.ViewOwn, L("Permission:Reviews.ViewOwn"));

        var templatesPermission = workManagementGroup.AddPermission(
            WorkManagementPermissions.Reviews.Templates.Default,
            L("Permission:Templates"));
        templatesPermission.AddChild(WorkManagementPermissions.Reviews.Templates.Create, L("Permission:Templates.Create"));
        templatesPermission.AddChild(WorkManagementPermissions.Reviews.Templates.Edit, L("Permission:Templates.Edit"));
        templatesPermission.AddChild(WorkManagementPermissions.Reviews.Templates.Delete, L("Permission:Templates.Delete"));
        templatesPermission.AddChild(WorkManagementPermissions.Reviews.Templates.ViewSystem, L("Permission:Templates.ViewSystem"));

        var analyticsPermission = workManagementGroup.AddPermission(
            WorkManagementPermissions.Reviews.Analytics.Default,
            L("Permission:Analytics"));
        analyticsPermission.AddChild(WorkManagementPermissions.Reviews.Analytics.ViewOwn, L("Permission:Analytics.ViewOwn"));

        // Task Management Permissions
        var tasksPermission = workManagementGroup.AddPermission(
            WorkManagementPermissions.TaskManagement.Tasks.Default,
            L("Permission:Tasks.Tasks"));
        tasksPermission.AddChild(
            WorkManagementPermissions.TaskManagement.Tasks.Create,
            L("Permission:Tasks.Tasks.Create"));
        tasksPermission.AddChild(
            WorkManagementPermissions.TaskManagement.Tasks.Update,
            L("Permission:Tasks.Tasks.Update"));
        tasksPermission.AddChild(
            WorkManagementPermissions.TaskManagement.Tasks.Delete,
            L("Permission:Tasks.Tasks.Delete"));
        tasksPermission.AddChild(
            WorkManagementPermissions.TaskManagement.Tasks.ManageActionSteps,
            L("Permission:Tasks.Tasks.ManageActionSteps"));
        tasksPermission.AddChild(
            WorkManagementPermissions.TaskManagement.Tasks.ManageCriteria,
            L("Permission:Tasks.Tasks.ManageCriteria"));

        var instancesPermission = workManagementGroup.AddPermission(
            WorkManagementPermissions.TaskManagement.TaskInstances.Default,
            L("Permission:Tasks.TaskInstances"));
        instancesPermission.AddChild(
            WorkManagementPermissions.TaskManagement.TaskInstances.Create,
            L("Permission:Tasks.TaskInstances.Create"));
        instancesPermission.AddChild(
            WorkManagementPermissions.TaskManagement.TaskInstances.Update,
            L("Permission:Tasks.TaskInstances.Update"));
        instancesPermission.AddChild(
            WorkManagementPermissions.TaskManagement.TaskInstances.Delete,
            L("Permission:Tasks.TaskInstances.Delete"));
        instancesPermission.AddChild(
            WorkManagementPermissions.TaskManagement.TaskInstances.Start,
            L("Permission:Tasks.TaskInstances.Start"));
        instancesPermission.AddChild(
            WorkManagementPermissions.TaskManagement.TaskInstances.Complete,
            L("Permission:Tasks.TaskInstances.Complete"));
        instancesPermission.AddChild(
            WorkManagementPermissions.TaskManagement.TaskInstances.Delegate,
            L("Permission:Tasks.TaskInstances.Delegate"));
        instancesPermission.AddChild(
            WorkManagementPermissions.TaskManagement.TaskInstances.Block,
            L("Permission:Tasks.TaskInstances.Block"));
        instancesPermission.AddChild(
            WorkManagementPermissions.TaskManagement.TaskInstances.UpdateProgress,
            L("Permission:Tasks.TaskInstances.UpdateProgress"));

        var typesPermission = workManagementGroup.AddPermission(
            WorkManagementPermissions.TaskManagement.TaskTypes.Default,
            L("Permission:Tasks.TaskTypes"));
        typesPermission.AddChild(
            WorkManagementPermissions.TaskManagement.TaskTypes.Create,
            L("Permission:Tasks.TaskTypes.Create"));
        typesPermission.AddChild(
            WorkManagementPermissions.TaskManagement.TaskTypes.Update,
            L("Permission:Tasks.TaskTypes.Update"));
        typesPermission.AddChild(
            WorkManagementPermissions.TaskManagement.TaskTypes.Delete,
            L("Permission:Tasks.TaskTypes.Delete"));
        typesPermission.AddChild(
            WorkManagementPermissions.TaskManagement.TaskTypes.ManageCustomFields,
            L("Permission:Tasks.TaskTypes.ManageCustomFields"));

        // Workflow Permissions
        var workflowPermission = workManagementGroup.AddPermission(
            WorkManagementPermissions.Workflow.Workflows.Default,
            L("Permission:Workflow.Workflows"));
        workflowPermission.AddChild(
            WorkManagementPermissions.Workflow.Workflows.Create,
            L("Permission:Workflow.Workflows.Create"));
        workflowPermission.AddChild(
            WorkManagementPermissions.Workflow.Workflows.Update,
            L("Permission:Workflow.Workflows.Update"));
        workflowPermission.AddChild(
            WorkManagementPermissions.Workflow.Workflows.Delete,
            L("Permission:Workflow.Workflows.Delete"));
        workflowPermission.AddChild(
            WorkManagementPermissions.Workflow.Workflows.Activate,
            L("Permission:Workflow.Workflows.Activate"));
        workflowPermission.AddChild(
            WorkManagementPermissions.Workflow.Workflows.Deactivate,
            L("Permission:Workflow.Workflows.Deactivate"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<WorkManagementResource>(name);
    }
}
