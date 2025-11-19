using Microsoft.EntityFrameworkCore;
using NouAssist.WorkManagement.Approval.Aggregates;
using NouAssist.WorkManagement.EntityFrameworkCore.TaskManagement;
using NouAssist.WorkManagement.EntityFrameworkCore.TaskManagement.Configurations;
using NouAssist.WorkManagement.GTD.Aggregates;
using NouAssist.WorkManagement.GTD.Entities;
using NouAssist.WorkManagement.ProjectManagement.Aggregates;
using NouAssist.WorkManagement.ProjectManagement.Entities;
using NouAssist.WorkManagement.Reviews.Aggregates;
using NouAssist.WorkManagement.Reviews.Entities;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using NouAssist.WorkManagement.TaskManagement.Entities;
using NouAssist.WorkManagement.Workflow.Aggregates;
using NouAssist.WorkManagement.Workflow.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.WorkManagement.EntityFrameworkCore;

[ConnectionStringName(WorkManagementDbProperties.ConnectionStringName)]
public class WorkManagementDbContext : AbpDbContext<WorkManagementDbContext>, IWorkManagementDbContext
{
    public DbSet<Task> Tasks { get; set; }
    public DbSet<TaskInstance> TaskInstances { get; set; }
    public DbSet<TaskType> TaskTypes { get; set; }

    // Child entities
    public DbSet<ChecklistItem> ChecklistItems { get; set; }
    public DbSet<TaskChecklistItem> TaskChecklistItems { get; set; }
    public DbSet<AcceptanceCriterion> AcceptanceCriteria { get; set; }
    public DbSet<AcceptanceCriterionCompletion> AcceptanceCriterionCompletions { get; set; }
    public DbSet<TaskDependency> TaskDependencies { get; set; }
    public DbSet<TaskTypeCustomField> TaskTypeCustomFields { get; set; }

    // Project Aggregate Roots
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectSprint> Sprints { get; set; }

    // Project Entities
    public DbSet<ProjectDependency> ProjectDependencies { get; set; }
    public DbSet<ProjectSuccessCriterion> ProjectSuccessCriteria { get; set; }
    public DbSet<ProjectEvent> ProjectEvents { get; set; }
    public DbSet<ProjectRequirement> ProjectRequirements { get; set; }

    //Workflow
    public DbSet<WorkManagement.Workflow.Aggregates.Workflow> Workflows { get; set; }
    public DbSet<WorkflowScheme> WorkflowSchemes { get; set; }
    public DbSet<WorkflowCustomField> WorkflowCustomFields { get; set; }
    public DbSet<WorkflowStatus> WorkflowStatuses { get; set; }
    public DbSet<WorkflowTransition> WorkflowTransitions { get; set; }
    public DbSet<WorkflowAction> WorkflowActions { get; set; }
    public DbSet<WorkflowSchemeMapping> WorkflowSchemeMappings { get; set; }
    public DbSet<TransitionPermissionPolicy> TransitionPermissionPolicies { get; set; }

    // Approvals
    public DbSet<ApprovalWorkflow> ApprovalWorkflows { get; set; }
    public DbSet<ApprovalRequest> ApprovalRequests { get; set; }

    // GTD
    public DbSet<GtdHorizon> GtdHorizons { get; set; }
    public DbSet<GtdHorizonAlignment> GtdHorizonAlignments { get; set; }
    public DbSet<SomedayMaybeItem> SomedayMaybeItems { get; set; }
    public DbSet<SomedayMaybeReview> SomedayMaybeReviews { get; set; }
    public DbSet<TicklerItem> TicklerItems { get; set; }
    public DbSet<TicklerProcessingLog> TicklerProcessingLogs { get; set; }
    public DbSet<WaitingFor> WaitingFors { get; set; }
    public DbSet<Followup> Followups { get; set; }
    
    // Reviews
    public DbSet<Review> Reviews { get; set; }
    public DbSet<ReviewTemplate> ReviewTemplates { get; set; }
    public DbSet<ReviewProgress> ReviewProgresses { get; set; }
    public DbSet<ReviewItem> ReviewItems { get; set; }

    public WorkManagementDbContext(DbContextOptions<WorkManagementDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureWorkManagement();

        // Apply all configurations
        builder.ApplyConfiguration(new TaskAggregateConfiguration());
        builder.ApplyConfiguration(new TaskInstanceAggregateConfiguration());
        builder.ApplyConfiguration(new TaskTypeAggregateConfiguration());
        builder.ApplyConfiguration(new ChecklistItemConfiguration());
        builder.ApplyConfiguration(new TaskChecklistItemConfiguration());
        builder.ApplyConfiguration(new AcceptanceCriterionConfiguration());
        builder.ApplyConfiguration(new AcceptanceCriterionCompletionConfiguration());
        builder.ApplyConfiguration(new TaskDependencyConfiguration());
        builder.ApplyConfiguration(new TaskTypeCustomFieldConfiguration());
    }
}