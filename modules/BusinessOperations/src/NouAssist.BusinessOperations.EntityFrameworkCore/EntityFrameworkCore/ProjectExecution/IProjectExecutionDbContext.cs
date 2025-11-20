using Microsoft.EntityFrameworkCore;
using NouAssist.BusinessOperations.ProjectExecution.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.BusinessOperations.ProjectExecution.EntityFrameworkCore
{
    /// <summary>
    /// DbContext interface for Project Execution module
    /// </summary>
    [ConnectionStringName(ProjectExecutionDbProperties.ConnectionStringName)]
    public interface IProjectExecutionDbContext : IEfCoreDbContext
    {
        /// <summary>
        /// DbSet for Project aggregate
        /// </summary>
        DbSet<Project> Projects { get; }
        
        /// <summary>
        /// DbSet for Task aggregate
        /// </summary>
        DbSet<Task> Tasks { get; }
        
        /// <summary>
        /// DbSet for TimeLog aggregate
        /// </summary>
        DbSet<TimeLog> TimeLogs { get; }
    }
}
