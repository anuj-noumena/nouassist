using Microsoft.EntityFrameworkCore;
using NouAssist.CoreProductivity.ProjectManagement.Aggregates;
using NouAssist.CoreProductivity.ProjectManagement.Entities;
using NouAssist.Workflow.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.EntityFrameworkCore.Projects
{
    [ConnectionStringName("Projects")]
    public class ProjectsDbContext : AbpDbContext<ProjectsDbContext>, IProjectsDbContext
    {
        // Aggregate Roots
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectSprint> Sprints { get; set; }
        
        // Entities
        public DbSet<ProjectDependency> ProjectDependencies { get; set; }
        public DbSet<ProjectSuccessCriterion> ProjectSuccessCriteria { get; set; }
        public DbSet<ProjectEvent> ProjectEvents { get; set; }
        public DbSet<ProjectRequirement> ProjectRequirements { get; set; }

        public ProjectsDbContext(DbContextOptions<ProjectsDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // IMPORTANT: Call base first to apply ABP conventions
            base.OnModelCreating(builder);

            // Apply all configurations for this module
            builder.ConfigureProjects(this);
        }
    }
}
