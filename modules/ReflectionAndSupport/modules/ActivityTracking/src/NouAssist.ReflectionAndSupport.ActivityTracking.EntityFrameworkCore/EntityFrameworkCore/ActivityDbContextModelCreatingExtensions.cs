using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.ReflectionAndSupport.ActivityTracking.EntityFrameworkCore
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
