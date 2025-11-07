using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.SharedKernel.EntityFrameworkCore
{
    public static class CoreDbContextModelCreatingExtensions
    {
        public static void ConfigureCore(
            this ModelBuilder builder,
            CoreDbContext dbContext)
        {
            Check.NotNull(builder, nameof(builder));

            builder.ApplyConfigurationsFromAssembly(typeof(CoreDbContext).Assembly);
        }
    }
}
