using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.SharedKernel.EntityFrameworkCore;

public static class SharedKernelDbContextModelCreatingExtensions
{
    public static void ConfigureSharedKernel(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.ApplyConfigurationsFromAssembly(typeof(SharedKernelDbContext).Assembly);
    }
}
