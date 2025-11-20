using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Competitor.Dtos;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.MarketTrend.Dtos;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.CustomerSegment.Dtos;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.SwotAnalysis.Dtos;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Dashboard.Dtos
{
    /// <summary>
    /// DTO for market intelligence dashboard data
    /// </summary>
    public class MarketIntelligenceDashboardDto
    {
        /// <summary>
        /// Top competitors by threat level
        /// </summary>
        public List<CompetitorDto> TopCompetitors { get; set; }

        /// <summary>
        /// Competitors by threat level counts
        /// </summary>
        public Dictionary<string, int> CompetitorsByThreatLevel { get; set; }

        /// <summary>
        /// Recent competitor updates
        /// </summary>
        public List<CompetitorUpdateLogDto> RecentCompetitorUpdates { get; set; }

        /// <summary>
        /// High-impact market trends
        /// </summary>
        public List<MarketTrendDto> HighImpactTrends { get; set; }

        /// <summary>
        /// Market opportunity alerts
        /// </summary>
        public List<OpportunityAlertDto> OpportunityAlerts { get; set; }

        /// <summary>
        /// Primary customer segments
        /// </summary>
        public List<CustomerSegmentProfileDto> PrimarySegments { get; set; }

        /// <summary>
        /// Recent SWOT analyses
        /// </summary>
        public List<SwotAnalysisDto> RecentSwotAnalyses { get; set; }

        /// <summary>
        /// Dashboard last refresh time
        /// </summary>
        public DateTime LastRefreshed { get; set; }
    }
}
