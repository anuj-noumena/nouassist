namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Services
{
    /// <summary>
    /// Domain service for market opportunity alerting
    /// </summary>
    public class OpportunityAlertingService : DomainService, IOpportunityAlertingService
    {
        private readonly IMarketTrendRepository _marketTrendRepository;
        private readonly ICompetitorRepository _competitorRepository;
        private readonly ISwotAnalysisRepository _swotRepository;
        private readonly IDistributedEventBus _eventBus;
        private readonly ILogger<OpportunityAlertingService> _logger;

        public OpportunityAlertingService(
            IMarketTrendRepository marketTrendRepository,
            ICompetitorRepository competitorRepository,
            ISwotAnalysisRepository swotRepository,
            IDistributedEventBus eventBus,
            ILogger<OpportunityAlertingService> logger)
        {
            _marketTrendRepository = marketTrendRepository;
            _competitorRepository = competitorRepository;
            _swotRepository = swotRepository;
            _eventBus = eventBus;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<List<OpportunityAlert>> ScanForOpportunitiesAsync()
        {
            try
            {
                var opportunities = new List<OpportunityAlert>();

                // Get high impact trends
                var highImpactSpec = new HighImpactMarketTrendSpecification();
                var activeSpec = new ActiveMarketTrendSpecification();
                var highConfidenceSpec = new HighConfidenceMarketTrendSpecification(0.8m);
                
                var spec = highImpactSpec.And(activeSpec).And(highConfidenceSpec);
                var trends = await _marketTrendRepository.GetListAsync(spec.ToExpression());

                foreach (var trend in trends)
                {
                    if (trend.ShouldTriggerOpportunityAlert())
                    {
                        var alert = new OpportunityAlert
                        {
                            Id = GuidGenerator.Create(),
                            SourceId = trend.Id,
                            Title = trend.TrendDescription.Title,
                            Description = trend.TrendDescription.Description,
                            ImpactLevel = trend.ImpactAssessment.Level,
                            ConfidenceScore = trend.ImpactAssessment.ConfidenceScore,
                            RecommendedActions = trend.ImpactAssessment.RecommendedActions,
                            Category = trend.Category,
                            AlertType = "Trend-Based Opportunity",
                            CreatedAt = Clock.Now
                        };

                        opportunities.Add(alert);
                    }
                }

                return opportunities;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to scan for market opportunities");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<ThreatAlert>> AnalyzeCompetitorThreatsAsync()
        {
            try
            {
                var threats = new List<ThreatAlert>();

                // Get high threat competitors with recent updates
                var highThreatSpec = new HighThreatCompetitorSpecification();
                var recentlyUpdatedSpec = new RecentlyUpdatedCompetitorSpecification(30);
                var activeSpec = new ActiveCompetitorSpecification();
                
                var spec = highThreatSpec.And(activeSpec).And(recentlyUpdatedSpec);
                var competitors = await _competitorRepository.GetListAsync(spec.ToExpression());

                foreach (var competitor in competitors)
                {
                    var recentHighImpactUpdates = competitor.UpdateLogs
                        .Where(u => u.UpdateDate >= Clock.Now.AddDays(-30) && 
                                  (u.ImpactLevel == ImpactLevel.High || u.ImpactLevel == ImpactLevel.Critical))
                        .OrderByDescending(u => u.UpdateDate)
                        .ToList();

                    if (recentHighImpactUpdates.Any())
                    {
                        var latestUpdate = recentHighImpactUpdates.First();
                        
                        var alert = new ThreatAlert
                        {
                            Id = GuidGenerator.Create(),
                            CompetitorId = competitor.Id,
                            CompetitorName = competitor.Name,
                            ThreatLevel = competitor.ThreatLevel,
                            UpdateType = latestUpdate.Type,
                            Title = latestUpdate.Title,
                            Description = latestUpdate.Description,
                            ImpactLevel = latestUpdate.ImpactLevel,
                            AlertDate = Clock.Now,
                            Source = latestUpdate.Source
                        };

                        threats.Add(alert);
                    }
                }

                return threats;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to analyze competitor threats");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<StrategicRecommendation>> GenerateStrategicRecommendationsAsync()
        {
            try
            {
                var recommendations = new List<StrategicRecommendation>();

                // Get completed SWOT analyses
                var completedAnalyses = await _swotRepository.GetCompletedAsync();
                var recentAnalyses = completedAnalyses
                    .Where(a => a.CompletedDate >= Clock.Now.AddDays(-90))
                    .OrderByDescending(a => a.CompletedDate)
                    .ToList();

                foreach (var analysis in recentAnalyses)
                {
                    // Get top threats
                    var topThreats = analysis
                        .GetItemsByCategory(SwotCategory.Threat)
                        .OrderByDescending(i => i.Impact)
                        .Take(3)
                        .ToList();

                    // Get top opportunities
                    var topOpportunities = analysis
                        .GetItemsByCategory(SwotCategory.Opportunity)
                        .OrderByDescending(i => i.Impact)
                        .Take(3)
                        .ToList();

                    if (topThreats.Any() && topOpportunities.Any())
                    {
                        var recommendation = new StrategicRecommendation
                        {
                            Id = GuidGenerator.Create(),
                            SourceAnalysisId = analysis.Id,
                            Title = $"Strategic Recommendations from {analysis.Title}",
                            Description = analysis.StrategicRecommendation,
                            GeneratedDate = Clock.Now,
                            TopThreats = topThreats.Select(t => new StrategicItem
                            {
                                Title = t.Title,
                                Description = t.Description,
                                Impact = t.Impact,
                                ActionItems = t.ActionItems
                            }).ToList(),
                            TopOpportunities = topOpportunities.Select(o => new StrategicItem
                            {
                                Title = o.Title,
                                Description = o.Description,
                                Impact = o.Impact,
                                ActionItems = o.ActionItems
                            }).ToList()
                        };

                        recommendations.Add(recommendation);
                    }
                }

                return recommendations;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate strategic recommendations");
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task PublishOpportunityAlertAsync(OpportunityAlert alert)
        {
            try
            {
                // Convert to event object
                var opportunityEvent = new OpportunityIdentifiedEto
                {
                    Id = alert.SourceId,
                    Title = alert.Title,
                    Description = alert.Description,
                    ImpactLevel = alert.ImpactLevel,
                    Source = alert.AlertType,
                    RecommendedActions = alert.RecommendedActions,
                    TenantId = CurrentTenant.Id,
                    EventTime = Clock.Now
                };

                // Publish the event
                await _eventBus.PublishAsync(opportunityEvent);
                
                _logger.LogInformation("Published opportunity alert: {Title}", alert.Title);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to publish opportunity alert {Title}", alert.Title);
                throw;
            }
        }
    }

    /// <summary>
    /// Market opportunity alert
    /// </summary>
    public class OpportunityAlert
    {
        public Guid Id { get; set; }
        public Guid SourceId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ImpactLevel ImpactLevel { get; set; }
        public decimal ConfidenceScore { get; set; }
        public string RecommendedActions { get; set; }
        public TrendCategory Category { get; set; }
        public string AlertType { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// Competitor threat alert
    /// </summary>
    public class ThreatAlert
    {
        public Guid Id { get; set; }
        public Guid CompetitorId { get; set; }
        public string CompetitorName { get; set; }
        public CompetitorThreatLevel ThreatLevel { get; set; }
        public UpdateType UpdateType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ImpactLevel ImpactLevel { get; set; }
        public DateTime AlertDate { get; set; }
        public string Source { get; set; }
    }

    /// <summary>
    /// Strategic recommendation
    /// </summary>
    public class StrategicRecommendation
    {
        public Guid Id { get; set; }
        public Guid SourceAnalysisId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime GeneratedDate { get; set; }
        public List<StrategicItem> TopThreats { get; set; }
        public List<StrategicItem> TopOpportunities { get; set; }
    }

    /// <summary>
    /// Strategic item
    /// </summary>
    public class StrategicItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Impact { get; set; }
        public string ActionItems { get; set; }
    }
}
