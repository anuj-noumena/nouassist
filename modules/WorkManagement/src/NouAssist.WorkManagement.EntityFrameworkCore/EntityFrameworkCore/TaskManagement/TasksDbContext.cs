using Microsoft.EntityFrameworkCore;
using NouAssist.WorkManagement.EntityFrameworkCore.TaskManagement.Configurations;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using NouAssist.WorkManagement.TaskManagement.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.WorkManagement.EntityFrameworkCore.TaskManagement
{
    [ConnectionStringName(TasksDbProperties.ConnectionStringName)]
    public class TasksDbContext : AbpDbContext<TasksDbContext>, ITasksDbContext
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

        public TasksDbContext(DbContextOptions<TasksDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureTasks();
            
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
}