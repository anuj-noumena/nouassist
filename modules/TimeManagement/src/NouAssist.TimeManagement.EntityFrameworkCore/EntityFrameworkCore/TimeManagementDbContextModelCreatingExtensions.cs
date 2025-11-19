using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.TimeManagement.EntityFrameworkCore;

public static class TimeManagementDbContextModelCreatingExtensions
{
    public static void ConfigureTimeManagement(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));
        builder.ApplyConfigurationsFromAssembly(typeof(TimeManagementDbContext).Assembly);

    }
}
