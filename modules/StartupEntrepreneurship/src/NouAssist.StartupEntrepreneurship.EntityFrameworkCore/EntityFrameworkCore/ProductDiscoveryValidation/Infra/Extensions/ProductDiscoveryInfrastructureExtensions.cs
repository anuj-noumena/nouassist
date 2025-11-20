namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Extensions
{
    /// <summary>
    /// Extension methods for Product Discovery infrastructure configuration
    /// </summary>
    public static class ProductDiscoveryInfrastructureExtensions
    {
        /// <summary>
        /// Configures Product Discovery EF Core with PostgreSQL
        /// </summary>
        public static IServiceCollection AddProductDiscoveryEntityFrameworkCore(
            this IServiceCollection services,
            string connectionString,
            Action<DbContextOptionsBuilder>? configureOptions = null)
        {
            services.AddAbpDbContext<ProductDiscoveryDbContext>(options =>
            {
                options.AddDefaultRepositories(includeAllEntities: true);
                
                // Configure PostgreSQL
                options.DbContextOptions.UseNpgsql(connectionString, npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsHistoryTable("__ProductDiscoveryMigrations");
                    npgsqlOptions.EnableRetryOnFailure(maxRetryCount: 3);
                    npgsqlOptions.CommandTimeout(30);
                });

                // Apply additional configuration
                configureOptions?.Invoke(options.DbContextOptions);
            });

            return services;
        }

        /// <summary>
        /// Configures Product Discovery database performance optimizations
        /// </summary>
        public static void ConfigureProductDiscoveryPerformance(this DbContextOptionsBuilder options)
        {
            options.EnableSensitiveDataLogging(false);
            options.EnableServiceProviderCaching();
            options.EnableDetailedErrors(false);
            
            // Configure connection pooling
            options.UseNpgsql(options => 
            {
                options.EnableRetryOnFailure(maxRetryCount: 3);
                options.CommandTimeout(30);
            });
        }

        /// <summary>
        /// Applies Product Discovery database migrations
        /// </summary>
        public static async Task ApplyProductDiscoveryMigrationsAsync(
            this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ProductDiscoveryDbContext>();
            
            await context.Database.MigrateAsync();
        }

        /// <summary>
        /// Seeds initial Product Discovery data
        /// </summary>
        public static async Task SeedProductDiscoveryDataAsync(
            this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var dataSeeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
            
            await dataSeeder.SeedAsync(new DataSeedContext(serviceProvider));
        }
    }
}
