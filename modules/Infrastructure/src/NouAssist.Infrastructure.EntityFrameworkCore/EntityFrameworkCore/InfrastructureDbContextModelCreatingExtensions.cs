using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.Infrastructure.EntityFrameworkCore;

public static class InfrastructureDbContextModelCreatingExtensions
{
    public static void ConfigureInfrastructure(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.ApplyConfigurationsFromAssembly(typeof(InfrastructureDbContext).Assembly);
    }
}
