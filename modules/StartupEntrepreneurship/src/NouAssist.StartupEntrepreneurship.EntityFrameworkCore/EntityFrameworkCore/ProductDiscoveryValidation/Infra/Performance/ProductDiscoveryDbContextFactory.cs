namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Performance
{
    /// <summary>
    /// Factory for creating optimized ProductDiscoveryDbContext instances
    /// </summary>
    public class ProductDiscoveryDbContextFactory : IDbContextFactory<ProductDiscoveryDbContext>
    {
        private readonly IDbContextFactory<ProductDiscoveryDbContext> _innerFactory;
        private readonly ILogger<ProductDiscoveryDbContextFactory> _logger;

        public ProductDiscoveryDbContextFactory(
            IDbContextFactory<ProductDiscoveryDbContext> innerFactory,
            ILogger<ProductDiscoveryDbContextFactory> logger)
        {
            _innerFactory = innerFactory;
            _logger = logger;
        }

        public ProductDiscoveryDbContext CreateDbContext()
        {
            var context = _innerFactory.CreateDbContext();
            
            // Configure performance settings
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            context.ChangeTracker.AutoDetectChangesEnabled = false;
            context.ChangeTracker.LazyLoadingEnabled = false;

            _logger.LogDebug("Created optimized ProductDiscoveryDbContext instance");
            
            return context;
        }

        public async Task<ProductDiscoveryDbContext> CreateDbContextAsync()
        {
            return await Task.FromResult(CreateDbContext());
        }
    }
}
