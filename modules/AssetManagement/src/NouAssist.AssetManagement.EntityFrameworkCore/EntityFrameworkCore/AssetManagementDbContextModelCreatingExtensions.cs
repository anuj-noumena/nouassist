using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.AssetManagement.EntityFrameworkCore;

public static class AssetManagementDbContextModelCreatingExtensions
{
    public static void ConfigureAssetManagement(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.ApplyConfigurationsFromAssembly(typeof(AssetManagementDbContext).Assembly);
    }
}
