using Microsoft.EntityFrameworkCore;
using NouAssist.Tasks.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Tasks.EntityFrameworkCore
{
    [ConnectionStringName(TasksDbProperties.ConnectionStringName)]
    public interface ITasksDbContext : IEfCoreDbContext
    {
        DbSet<Task> Tasks { get; }
        DbSet<TaskInstance> TaskInstances { get; }
        DbSet<TaskType> TaskTypes { get; }
    }
}
