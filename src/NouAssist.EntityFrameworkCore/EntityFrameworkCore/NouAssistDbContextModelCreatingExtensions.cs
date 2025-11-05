using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.EntityFrameworkCore
{
    public static class NouAssistDbContextModelCreatingExtensions
    {
        public static void ConfigureTasks(
            this ModelBuilder builder,
            NouAssistDbContext dbContext)
        {
            Check.NotNull(builder, nameof(builder));

            // Apply all IEntityTypeConfiguration classes from this assembly
            builder.ApplyConfigurationsFromAssembly(typeof(NouAssistDbContext).Assembly);
        }
    }
}
