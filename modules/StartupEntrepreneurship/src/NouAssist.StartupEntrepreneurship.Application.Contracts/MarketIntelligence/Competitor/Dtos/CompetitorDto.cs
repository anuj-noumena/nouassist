using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Competitor.Dtos
{
    /// <summary>
    /// Data transfer object for a competitor
    /// </summary>
    public class CompetitorDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// Competitor name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Competitor description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Competitor status
        /// </summary>
        public CompetitorStatus Status { get; set; }

        /// <summary>
        /// Threat level posed by the competitor
        /// </summary>
        public CompetitorThreatLevel ThreatLevel { get; set; }

        /// <summary>
        /// Notes about the competitor
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Tags for the competitor
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// Date when the competitor was last analyzed
        /// </summary>
        public DateTime LastAnalyzed { get; set; }

        /// <summary>
        /// Competitive score (0-1 scale)
        /// </summary>
        public decimal CompetitiveScore { get; set; }

        /// <summary>
        /// Competitor information (website, industry, etc.)
        /// </summary>
        public CompetitorInfoDto CompetitorInfo { get; set; }

        /// <summary>
        /// Recent updates for the competitor
        /// </summary>
        public List<CompetitorUpdateLogDto> RecentUpdateLogs { get; set; }
    }
}
