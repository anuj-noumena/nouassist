namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.EntityFrameworkCore
{
    /// <summary>
    /// Extension methods for configuring Product Discovery EF Core model
    /// </summary>
    public static class ProductDiscoveryDbContextModelCreatingExtensions
    {
        public static void ConfigureProductDiscovery(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            // Apply all IEntityTypeConfiguration classes defined in this assembly
            builder.ApplyConfigurationsFromAssembly(typeof(ProductDiscoveryDbContext).Assembly);
        }
    }
}
