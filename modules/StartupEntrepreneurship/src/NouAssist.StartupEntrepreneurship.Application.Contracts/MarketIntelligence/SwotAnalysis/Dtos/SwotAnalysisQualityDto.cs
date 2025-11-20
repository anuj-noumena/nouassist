using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.SwotAnalysis.Dtos
{
    /// <summary>
    /// DTO for SWOT analysis quality evaluation results
    /// </summary>
    public class SwotAnalysisQualityDto : EntityDto<Guid>
    {
        /// <summary>
        /// Analysis title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Balance score (0-1 scale)
        /// </summary>
        public decimal BalanceScore { get; set; }

        /// <summary>
        /// Category coverage percentages
        /// </summary>
        public Dictionary<SwotCategory, decimal> CategoryCoverage { get; set; }

        /// <summary>
        /// Number of items in the analysis
        /// </summary>
        public int ItemCount { get; set; }

        /// <summary>
        /// Whether the analysis is complete
        /// </summary>
        public bool IsComplete { get; set; }

        /// <summary>
        /// Missing categories
        /// </summary>
        public List<SwotCategory> MissingCategories { get; set; }
    }
}
