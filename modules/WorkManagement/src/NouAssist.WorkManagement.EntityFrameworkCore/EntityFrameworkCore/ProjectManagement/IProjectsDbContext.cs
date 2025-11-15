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
        
        // Entities (if not configured as owned entities)
        DbSet<ProjectDependency> ProjectDependencies { get; }
        DbSet<ProjectEvent> ProjectEvents { get; }
        DbSet<ProjectRequirement> ProjectRequirements { get; }
        
        // Note: ProjectSuccessCriterion can be accessed through Project.SuccessCriteria
        // If you need direct access, uncomment:
        // DbSet<ProjectSuccessCriterion> ProjectSuccessCriteria { get; }
    }
}