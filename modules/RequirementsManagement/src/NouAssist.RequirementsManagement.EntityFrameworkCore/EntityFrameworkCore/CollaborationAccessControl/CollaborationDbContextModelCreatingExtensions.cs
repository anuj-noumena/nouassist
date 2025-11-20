using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.RequirementsManagement.CollaborationAccessControl.EntityFrameworkCore
{
    public static class CollaborationDbContextModelCreatingExtensions
    {
        public static void ConfigureCollaboration(
            this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            // Apply all IEntityTypeConfiguration classes defined in this assembly
            builder.ApplyConfigurationsFromAssembly(typeof(CollaborationDbContextModelCreatingExtensions).Assembly);
        }
    }
}
