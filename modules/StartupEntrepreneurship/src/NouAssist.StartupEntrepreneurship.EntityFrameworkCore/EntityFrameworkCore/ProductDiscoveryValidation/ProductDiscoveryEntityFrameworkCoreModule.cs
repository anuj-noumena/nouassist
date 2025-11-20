namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.EntityFrameworkCore
{
    /// <summary>
    /// EF Core module for Product Discovery
    /// </summary>
    [DependsOn(
        typeof(ProductDiscoveryDomainModule),
        typeof(AbpEntityFrameworkCorePostgreSQLModule)
    )]
    public class ProductDiscoveryEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // Configure connection string
            context.Services.AddAbpDbContext<ProductDiscoveryDbContext>(options =>
            {
                // Register custom repositories
                options.AddRepository<CustomerInterview, EfCoreCustomerInterviewRepository>();
                options.AddRepository<ProblemHypothesis, EfCoreProblemHypothesisRepository>();
                options.AddRepository<SolutionPrototype, EfCoreSolutionPrototypeRepository>();
                options.AddRepository<UserFeedbackSession, EfCoreUserFeedbackSessionRepository>();
                options.AddRepository<FeatureDefinition, EfCoreFeatureDefinitionRepository>();
                options.AddRepository<CustomerSegment, EfCoreCustomerSegmentRepository>();

                // Configure default repositories for child entities if needed
                options.AddDefaultRepositories(includeAllEntities: false);
            });

            // Configure EF Core options
            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql();
            });
        }
    }
}
