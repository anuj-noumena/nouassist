using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.Infrastructure.Activity.EntityFrameworkCore
{
    public static class ActivityDbContextModelCreatingExtensions
    {
        public static void ConfigureActivity(
            this ModelBuilder builder,
            ActivityDbContext dbContext)
        {
            Check.NotNull(builder, nameof(builder));

            builder.ApplyConfigurationsFromAssembly(typeof(ActivityDbContext).Assembly);
        }
    }
}
