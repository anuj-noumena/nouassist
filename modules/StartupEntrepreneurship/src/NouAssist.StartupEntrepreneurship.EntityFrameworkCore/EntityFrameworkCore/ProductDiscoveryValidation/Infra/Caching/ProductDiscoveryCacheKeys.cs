namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Caching
{
    /// <summary>
    /// Cache key constants for Product Discovery module
    /// </summary>
    public static class ProductDiscoveryCacheKeys
    {
        public const string InterviewStatistics = "ProductDiscovery:InterviewStatistics";
        public const string FeatureStatistics = "ProductDiscovery:FeatureStatistics";
        public const string PrototypeStatistics = "ProductDiscovery:PrototypeStatistics";
        public const string ValidationPortfolioSummary = "ProductDiscovery:ValidationPortfolioSummary";
        public const string SegmentCompletionSummary = "ProductDiscovery:SegmentCompletionSummary";

        public static string GetInterviewsBySegment(Guid segmentId) => 
            $"ProductDiscovery:InterviewsBySegment:{segmentId}";

        public static string GetHypothesesBySegment(Guid segmentId) => 
            $"ProductDiscovery:HypothesesBySegment:{segmentId}";

        public static string GetFeaturesByHypothesis(Guid hypothesisId) => 
            $"ProductDiscovery:FeaturesByHypothesis:{hypothesisId}";

        public static string GetPrototypesByHypothesis(Guid hypothesisId) => 
            $"ProductDiscovery:PrototypesByHypothesis:{hypothesisId}";

        public static string GetFeedbackSessionsByPrototype(Guid prototypeId) => 
            $"ProductDiscovery:FeedbackSessionsByPrototype:{prototypeId}";
    }
}
