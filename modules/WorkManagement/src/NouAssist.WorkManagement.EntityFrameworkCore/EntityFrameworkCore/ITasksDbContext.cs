using Microsoft.EntityFrameworkCore;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
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
    }
}
