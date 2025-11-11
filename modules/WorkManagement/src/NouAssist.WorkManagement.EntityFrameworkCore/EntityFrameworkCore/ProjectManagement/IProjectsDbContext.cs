using Microsoft.EntityFrameworkCore;
using NouAssist.WorkManagement.ProjectManagement.Aggregates;
using NouAssist.WorkManagement.ProjectManagement.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.WorkManagement.EntityFrameworkCore.ProjectManagement
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
