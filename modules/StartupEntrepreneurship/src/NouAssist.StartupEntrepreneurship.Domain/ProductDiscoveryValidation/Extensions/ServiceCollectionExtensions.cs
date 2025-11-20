namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Extensions
{
    /// <summary>
    /// Extension methods for IServiceCollection
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Product Discovery domain services to the service collection
        /// </summary>
        public static IServiceCollection AddProductDiscoveryDomain(this IServiceCollection services)
        {
            // Register domain services
            services.AddTransient<IInsightAggregationService, InsightAggregationService>();
            services.AddTransient<IHypothesisValidationTracker, HypothesisValidationTracker>();
            services.AddTransient<IFeedbackAnalysisService, FeedbackAnalysisService>();
            services.AddTransient<IFeaturePrioritizationEngine, FeaturePrioritizationEngine>();
            services.AddTransient<IProblemSolutionFitCalculator, ProblemSolutionFitCalculator>();

            return services;
        }

        /// <summary>
        /// Adds Product Discovery specifications to the service collection
        /// </summary>
        public static IServiceCollection AddProductDiscoverySpecifications(this IServiceCollection services)
        {
            // Register specifications as transient services
            services.AddTransient<HighValueInterviewSpecification>();
            services.AddTransient<InterviewInSegmentSpecification>();
            services.AddTransient<StronglyValidatedHypothesisSpecification>();
            services.AddTransient<ReadyForValidationSpecification>();
            services.AddTransient<MvpReadyFeatureSpecification>();
            services.AddTransient<HighImpactFeatureSpecification>();
            services.AddTransient<TestReadyPrototypeSpecification>();
            services.AddTransient<ComprehensiveFeedbackSpecification>();

            return services;
        }
    }
}
