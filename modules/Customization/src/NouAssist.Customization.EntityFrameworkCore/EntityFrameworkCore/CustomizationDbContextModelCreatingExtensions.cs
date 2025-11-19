using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NouAssist.Customization.EntityFrameworkCore;

public static class CustomizationDbContextModelCreatingExtensions
{
    public static void ConfigureCustomization(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.ApplyConfigurationsFromAssembly(typeof(CustomizationDbContext).Assembly);
    }
}
