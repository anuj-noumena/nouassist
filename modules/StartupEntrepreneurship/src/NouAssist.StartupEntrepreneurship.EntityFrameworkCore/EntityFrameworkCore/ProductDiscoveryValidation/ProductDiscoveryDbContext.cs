namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.EntityFrameworkCore
{
    /// <summary>
    /// DbContext implementation for Product Discovery module
    /// </summary>
    [ConnectionStringName(ProductDiscoveryDbProperties.ConnectionStringName)]
    public class ProductDiscoveryDbContext 
        : AbpDbContext<ProductDiscoveryDbContext>, IProductDiscoveryDbContext
    {
        // Implement DbSet properties from the interface
        public DbSet<CustomerInterview> CustomerInterviews { get; set; } = null!;
        public DbSet<ProblemHypothesis> ProblemHypotheses { get; set; } = null!;
        public DbSet<SolutionPrototype> SolutionPrototypes { get; set; } = null!;
        public DbSet<UserFeedbackSession> UserFeedbackSessions { get; set; } = null!;
        public DbSet<FeatureDefinition> FeatureDefinitions { get; set; } = null!;
        public DbSet<CustomerSegment> CustomerSegments { get; set; } = null!;

        public ProductDiscoveryDbContext(DbContextOptions<ProductDiscoveryDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // IMPORTANT: Call base first to apply ABP conventions
            base.OnModelCreating(builder);

            // Apply all configurations defined in this assembly
            builder.ConfigureProductDiscovery();
        }
    }
}
