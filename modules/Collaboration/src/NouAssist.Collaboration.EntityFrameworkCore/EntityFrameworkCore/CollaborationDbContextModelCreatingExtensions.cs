using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.Collaboration.EntityFrameworkCore;

public static class CollaborationDbContextModelCreatingExtensions
{
    public static void ConfigureCollaboration(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.ApplyConfigurationsFromAssembly(typeof(CollaborationDbContext).Assembly);
    }
}
