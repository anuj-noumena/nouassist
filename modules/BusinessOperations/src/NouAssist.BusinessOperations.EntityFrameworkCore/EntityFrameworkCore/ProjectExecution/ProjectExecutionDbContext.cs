using Microsoft.EntityFrameworkCore;
using NouAssist.BusinessOperations.ProjectExecution.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.BusinessOperations.ProjectExecution.EntityFrameworkCore
{
    /// <summary>
    /// DbContext implementation for Project Execution module
    /// </summary>
    [ConnectionStringName(ProjectExecutionDbProperties.ConnectionStringName)]
    public class ProjectExecutionDbContext : AbpDbContext<ProjectExecutionDbContext>, IProjectExecutionDbContext
    {
        #region DbSets
        
        /// <summary>
        /// DbSet for Project aggregate
        /// </summary>
        public DbSet<Project> Projects { get; set; }
        
        /// <summary>
        /// DbSet for Task aggregate
        /// </summary>
        public DbSet<Task> Tasks { get; set; }
        
        /// <summary>
        /// DbSet for TimeLog aggregate
        /// </summary>
        public DbSet<TimeLog> TimeLogs { get; set; }
        
        #endregion
        
        /// <summary>
        /// Creates a new instance of ProjectExecutionDbContext
        /// </summary>
        public ProjectExecutionDbContext(DbContextOptions<ProjectExecutionDbContext> options)
            : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Call base method first to apply ABP conventions
            base.OnModelCreating(builder);
            
            // Apply all configurations for this module
            builder.ConfigureProjectExecution();
        }
    }
}
