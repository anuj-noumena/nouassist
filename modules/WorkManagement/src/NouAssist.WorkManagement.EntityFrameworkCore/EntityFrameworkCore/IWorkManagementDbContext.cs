using Microsoft.EntityFrameworkCore;
using NouAssist.WorkManagement.Approval.Aggregates;
using NouAssist.WorkManagement.GTD.Aggregates;
using NouAssist.WorkManagement.GTD.Entities;
using NouAssist.WorkManagement.ProjectManagement.Aggregates;
using NouAssist.WorkManagement.ProjectManagement.Entities;
using NouAssist.WorkManagement.Reviews.Aggregates;
using NouAssist.WorkManagement.Reviews.Entities;
using NouAssist.WorkManagement.TaskManagement.Aggregates.Task;
using NouAssist.WorkManagement.TaskManagement.Aggregates.TaskInstance;
using NouAssist.WorkManagement.TaskManagement.Entities;
using NouAssist.WorkManagement.Workflow.Aggregates;
using NouAssist.WorkManagement.Workflow.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.WorkManagement.EntityFrameworkCore;

[ConnectionStringName(WorkManagementDbProperties.ConnectionStringName)]
public interface IWorkManagementDbContext : IEfCoreDbContext
{
    // Task Aggregate Roots
    DbSet<TaskAggregate> Tasks { get; }
    DbSet<TaskDefinitionAggregate> TaskDefinitions { get; }
    DbSet<TaskDefaultsAggregate> TaskDefaults { get; }
    DbSet<TaskRecurrenceAggregate> TaskRecurrences { get; }
    DbSet<TaskHierarchyAggregate> TaskHierarchies { get; }
    DbSet<TaskType> TaskTypes { get; }

    // TaskInstance Aggregate Roots
    DbSet<TaskInstance> TaskInstances { get; }
    DbSet<TaskChecklistAggregate> TaskChecklists { get; }
    DbSet<TaskOrganizationAggregate> TaskOrganizations { get; }
    DbSet<TaskInstanceWorkAggregate> TaskInstanceWork { get; }
    DbSet<TaskCollaborationAggregate> TaskCollaborations { get; }
    DbSet<TaskFlowControlAggregate> TaskFlowControls { get; }

    // Child entities
    DbSet<ChecklistItem> ChecklistItems { get; }
    DbSet<TaskChecklistItem> TaskChecklistItems { get; }
    DbSet<AcceptanceCriterion> AcceptanceCriteria { get; }
    DbSet<AcceptanceCriterionCompletion> AcceptanceCriterionCompletions { get; }
    DbSet<TaskDependency> TaskDependencies { get; }
    DbSet<TaskTypeCustomField> TaskTypeCustomFields { get; }
    DbSet<TaskActionStep> TaskActionSteps { get; }
    DbSet<TaskActionStepCompletion> TaskActionStepCompletions { get; }
    
    // Project Aggregate Roots
    DbSet<Project> Projects { get; }
    DbSet<ProjectSprint> Sprints { get; }
        
    // Entities (if not configured as owned entities)
    DbSet<ProjectDependency> ProjectDependencies { get; }
    DbSet<ProjectEvent> ProjectEvents { get; }
    DbSet<ProjectRequirement> ProjectRequirements { get; }
    
    // Workflow
    DbSet<WorkManagement.Workflow.Aggregates.Workflow> Workflows { get; }
    DbSet<WorkflowScheme> WorkflowSchemes { get; }
    DbSet<WorkflowCustomField> WorkflowCustomFields { get; }
    DbSet<WorkflowStatus> WorkflowStatuses { get; }
    DbSet<WorkflowTransition> WorkflowTransitions { get; }
    DbSet<WorkflowAction> WorkflowActions { get; }
    DbSet<WorkflowSchemeMapping> WorkflowSchemeMappings { get; }
    DbSet<TransitionPermissionPolicy> TransitionPermissionPolicies { get; }
    
    // Approvals
    public DbSet<ApprovalWorkflow> ApprovalWorkflows { get; set; }
    public DbSet<ApprovalRequest> ApprovalRequests { get; set; }
    
    // GTD
    DbSet<GtdHorizon> GtdHorizons { get; }
    DbSet<GtdHorizonAlignment> GtdHorizonAlignments { get; }
    DbSet<SomedayMaybeItem> SomedayMaybeItems { get; }
    DbSet<SomedayMaybeReview> SomedayMaybeReviews { get; }
    DbSet<TicklerItem> TicklerItems { get; }
    DbSet<TicklerProcessingLog> TicklerProcessingLogs { get; }
    DbSet<WaitingFor> WaitingFors { get; }
    DbSet<Followup> Followups { get; }
    
    // Reviews
    DbSet<Review> Reviews { get; }
    DbSet<ReviewTemplate> ReviewTemplates { get; }
    DbSet<ReviewProgress> ReviewProgresses { get; }
    DbSet<ReviewItem> ReviewItems { get; }
}
