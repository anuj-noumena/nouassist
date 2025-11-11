using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.EntityFrameworkCore.Projects
{
    public static class ProjectsDbContextModelCreatingExtensions
    {
        public static void ConfigureProjects(
            this ModelBuilder builder,
            ProjectsDbContext dbContext)
        {
            Check.NotNull(builder, nameof(builder));

            // Apply all IEntityTypeConfiguration classes from this assembly
            builder.ApplyConfigurationsFromAssembly(typeof(ProjectsDbContext).Assembly);

            // Additional module-specific configurations can go here if needed
        }
    }
}
