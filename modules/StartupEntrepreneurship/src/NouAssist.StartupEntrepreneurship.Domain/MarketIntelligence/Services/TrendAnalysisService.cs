namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Services
{
    /// <summary>
    /// Domain service for market trend analysis
    /// </summary>
    public class TrendAnalysisService : DomainService, ITrendAnalysisService
    {
        private readonly IMarketTrendRepository _marketTrendRepository;
        private readonly ILogger<TrendAnalysisService> _logger;

        public TrendAnalysisService(
            IMarketTrendRepository marketTrendRepository,
            ILogger<TrendAnalysisService> logger)
        {
            _marketTrendRepository = marketTrendRepository;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<MarketTrend> CreateMarketTrendAsync(
            TrendDescription trendDescription,
            TrendCategory category,
            ImpactAssessment impactAssessment,
            DateTime? expiryDate = null)
        {
            try
            {
                var trend = MarketTrend.Create(
                    GuidGenerator.Create(),
                    trendDescription,
                    category,
                    impactAssessment,
                    Clock.Now,
                    expiryDate,
                    CurrentTenant.Id
                );

                await _marketTrendRepository.InsertAsync(trend);
                return trend;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create market trend with title {Title}", trendDescription.Title);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<MarketTrend> UpdateMarketTrendAsync(
            Guid trendId,
            TrendDescription trendDescription,
            ImpactAssessment impactAssessment,
            string tags = null)
        {
            try
            {
                var trend = await _marketTrendRepository.GetAsync(trendId);
                trend.UpdateTrend(trendDescription, impactAssessment, tags);
                await _marketTrendRepository.UpdateAsync(trend);
                return trend;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update market trend {TrendId}", trendId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task AddTrendDataSourceAsync(
            Guid trendId,
            string sourceName,
            string sourceUrl,
            string sourceType,
            decimal reliability,
            string dataQuality = null,
            string extractedData = null)
        {
            try
            {
                var trend = await _marketTrendRepository.GetAsync(trendId);
                
                trend.AddDataSource(
                    sourceName,
                    sourceUrl,
                    sourceType,
                    reliability,
                    Clock.Now,
                    dataQuality,
                    extractedData
                );

                await _marketTrendRepository.UpdateAsync(trend);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add data source for trend {TrendId}", trendId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<MarketTrend>> GetHighImpactTrendsAsync()
        {
            try
            {
                var activeSpec = new ActiveMarketTrendSpecification();
                var highImpactSpec = new HighImpactMarketTrendSpecification();
                var combinedSpec = activeSpec.And(highImpactSpec);

                return await _marketTrendRepository.GetListAsync(combinedSpec.ToExpression());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get high impact trends");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<OpportunityResult>> EvaluateMarketOpportunitiesAsync()
        {
            try
            {
                var results = new List<OpportunityResult>();
                
                // Get all active trends with high confidence and impact
                var activeSpec = new ActiveMarketTrendSpecification();
                var highConfidenceSpec = new HighConfidenceMarketTrendSpecification(0.7m);
                var highImpactSpec = new HighImpactMarketTrendSpecification();
                
                var spec = activeSpec.And(highConfidenceSpec).And(highImpactSpec);
                var candidates = await _marketTrendRepository.GetListAsync(spec.ToExpression());

                foreach (var trend in candidates)
                {
                    if (trend.ShouldTriggerOpportunityAlert())
                    {
                        results.Add(new OpportunityResult
                        {
                            Id = trend.Id,
                            Title = trend.TrendDescription.Title,
                            Description = trend.TrendDescription.Description,
                            ImpactLevel = trend.ImpactAssessment.Level,
                            ConfidenceScore = trend.ImpactAssessment.ConfidenceScore,
                            RecommendedActions = trend.ImpactAssessment.RecommendedActions,
                            Category = trend.Category,
                            ReliabilityScore = trend.GetAverageReliability()
                        });
                    }
                }

                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to evaluate market opportunities");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<MarketTrend>> GetActiveTrendsByCategoryAsync(TrendCategory category)
        {
            try
            {
                var activeSpec = new ActiveMarketTrendSpecification();
                var categorySpec = new TrendInCategorySpecification(category);
                var combinedSpec = activeSpec.And(categorySpec);

                return await _marketTrendRepository.GetListAsync(combinedSpec.ToExpression());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get trends for category {Category}", category);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task DeactivateExpiredTrendsAsync()
        {
            try
            {
                var activeTrends = await _marketTrendRepository.GetActiveAsync();
                var now = Clock.Now;
                var expiredTrends = activeTrends.Where(t => t.ExpiryDate.HasValue && t.ExpiryDate.Value <= now).ToList();

                foreach (var trend in expiredTrends)
                {
                    trend.DeactivateTrend("Automatically deactivated due to expiry date");
                }

                if (expiredTrends.Any())
                {
                    await _marketTrendRepository.UpdateManyAsync(expiredTrends);
                    _logger.LogInformation("Deactivated {Count} expired trends", expiredTrends.Count);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to deactivate expired trends");
                throw;
            }
        }
    }

    /// <summary>
    /// Result class for opportunity evaluation
    /// </summary>
    public class OpportunityResult
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ImpactLevel ImpactLevel { get; set; }
        public decimal ConfidenceScore { get; set; }
        public string RecommendedActions { get; set; }
        public TrendCategory Category { get; set; }
        public decimal ReliabilityScore { get; set; }
    }
}
