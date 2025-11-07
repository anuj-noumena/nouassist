using Microsoft.EntityFrameworkCore;
using NouAssist.Projects.Aggregates;
using NouAssist.Projects.Entities;
using NouAssist.Workflow.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.EntityFrameworkCore.Projects
{
    [ConnectionStringName("Projects")]
    public interface IProjectsDbContext : IEfCoreDbContext
    {
        // Aggregate Roots
        DbSet<Project> Projects { get; }
        DbSet<ProjectSprint> Sprints { get; }
        
        // Entities
        DbSet<ProjectDependency> ProjectDependencies { get; }
        DbSet<ProjectSuccessCriterion> ProjectSuccessCriteria { get; }
        DbSet<ProjectEvent> ProjectEvents { get; }
        DbSet<ProjectRequirement> ProjectRequirements { get; }
    }
}
