using Microsoft.EntityFrameworkCore;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using NouAssist.WorkManagement.TaskManagement.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.WorkManagement.EntityFrameworkCore.TaskManagement
{
    [ConnectionStringName(TasksDbProperties.ConnectionStringName)]
    public interface ITasksDbContext : IEfCoreDbContext
    {
        DbSet<Task> Tasks { get; }
        DbSet<TaskInstance> TaskInstances { get; }
        DbSet<TaskType> TaskTypes { get; }
        
        // Child entities
        DbSet<ChecklistItem> ChecklistItems { get; }
        DbSet<TaskChecklistItem> TaskChecklistItems { get; }
        DbSet<AcceptanceCriterion> AcceptanceCriteria { get; }
        DbSet<AcceptanceCriterionCompletion> AcceptanceCriterionCompletions { get; }
        DbSet<TaskDependency> TaskDependencies { get; }
        DbSet<TaskTypeCustomField> TaskTypeCustomFields { get; }
    }
}