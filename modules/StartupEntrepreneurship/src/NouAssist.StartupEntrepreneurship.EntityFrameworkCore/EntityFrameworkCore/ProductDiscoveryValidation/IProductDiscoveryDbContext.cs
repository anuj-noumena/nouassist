namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.EntityFrameworkCore
{
    /// <summary>
    /// DbContext interface for Product Discovery module
    /// </summary>
    [ConnectionStringName(ProductDiscoveryDbProperties.ConnectionStringName)]
    public interface IProductDiscoveryDbContext : IEfCoreDbContext
    {
        // DbSet for each Aggregate Root
        DbSet<CustomerInterview> CustomerInterviews { get; }
        DbSet<ProblemHypothesis> ProblemHypotheses { get; }
        DbSet<SolutionPrototype> SolutionPrototypes { get; }
        DbSet<UserFeedbackSession> UserFeedbackSessions { get; }
        DbSet<FeatureDefinition> FeatureDefinitions { get; }
        
        // DbSet for standalone entities
        DbSet<CustomerSegment> CustomerSegments { get; }
    }
}
