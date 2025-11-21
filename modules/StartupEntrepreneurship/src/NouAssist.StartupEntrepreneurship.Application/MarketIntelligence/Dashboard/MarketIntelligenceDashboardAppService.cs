using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Competitor;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Competitor.Dtos;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.CustomerSegment;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.CustomerSegment.Dtos;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Dashboard.Dtos;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Enums;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Interfaces.Services;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Localization;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.MarketTrend;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.MarketTrend.Dtos;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Permissions;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Repositories;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.SwotAnalysis;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.SwotAnalysis.Dtos;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Dashboard
{
    [Authorize(MarketIntelligencePermissions.Dashboard.View)]
    public class MarketIntelligenceDashboardAppService : ApplicationService, IMarketIntelligenceDashboardAppService
    {
        private readonly ICompetitorAppService _competitorAppService;
        private readonly IMarketTrendAppService _marketTrendAppService;
        private readonly ICustomerSegmentProfileAppService _customerSegmentAppService;
        private readonly ISwotAnalysisAppService _swotAnalysisAppService;
        private readonly IOpportunityAlertingService _opportunityAlertingService;
        private readonly ILogger<MarketIntelligenceDashboardAppService> _logger;

        public MarketIntelligenceDashboardAppService(
            ICompetitorAppService competitorAppService,
            IMarketTrendAppService marketTrendAppService,
            ICustomerSegmentProfileAppService customerSegmentAppService,
            ISwotAnalysisAppService swotAnalysisAppService,
            IOpportunityAlertingService opportunityAlertingService,
            ILogger<MarketIntelligenceDashboardAppService> logger)
        {
            _competitorAppService = competitorAppService;
            _marketTrendAppService = marketTrendAppService;
            _customerSegmentAppService = customerSegmentAppService;
            _swotAnalysisAppService = swotAnalysisAppService;
            _opportunityAlertingService = opportunityAlertingService;
            _logger = logger;
            
            LocalizationResource = typeof(MarketIntelligenceResource);
        }

        public virtual async Task<MarketIntelligenceDashboardDto> GetDashboardDataAsync()
        {
            try
            {
                var dashboard = new MarketIntelligenceDashboardDto
                {
                    LastRefreshed = Clock.Now
                };

                // Get top competitors by threat level
                var topCompetitors = await _competitorAppService.GetByThreatLevelAsync(CompetitorThreatLevel.High);
                topCompetitors.AddRange(await _competitorAppService.GetByThreatLevelAsync(CompetitorThreatLevel.Critical));
                dashboard.TopCompetitors = topCompetitors.OrderByDescending(c => c.ThreatLevel).Take(5).ToList();

                // Get competitors by threat level counts
                dashboard.CompetitorsByThreatLevel = await GetCompetitorCountsByThreatLevelAsync();

                // Get recent competitor updates
                var competitorsWithUpdates = await _competitorAppService.GetWithRecentUpdatesAsync(30, 5);
                dashboard.RecentCompetitorUpdates = competitorsWithUpdates
                    .SelectMany(c => c.RecentUpdateLogs)
                    .OrderByDescending(u => u.UpdateDate)
                    .Take(10)
                    .ToList();

                // Get high-impact market trends
                dashboard.HighImpactTrends = await _marketTrendAppService.GetHighImpactTrendsAsync();

                // Get market opportunity alerts
                dashboard.OpportunityAlerts = await _marketTrendAppService.GetMarketOpportunitiesAsync();

                // Get primary customer segments
                dashboard.PrimarySegments = await _customerSegmentAppService.GetPrimarySegmentsAsync();

                // Get recent SWOT analyses
                dashboard.RecentSwotAnalyses = await _swotAnalysisAppService.GetRecentAsync(3);

                return dashboard;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        private async Task<Dictionary<string, int>> GetCompetitorCountsByThreatLevelAsync()
        {
            var result = new Dictionary<string, int>();
            
            // Create list of all threat levels
            var threatLevels = Enum.GetValues(typeof(CompetitorThreatLevel)).Cast<CompetitorThreatLevel>();
            
            foreach (var level in threatLevels)
            {
                var competitors = await _competitorAppService.GetByThreatLevelAsync(level);
                result[level.ToString()] = competitors.Count;
            }
            
            return result;
        }

        // Error handling helper
        protected virtual Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            Logger.Log(logLevel, exception, "Error in MarketIntelligenceDashboardAppService: {ErrorMessage}", exception.Message);
            return Task.CompletedTask;
        }
    }
}
