using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.MarketTrend.Dtos
{
    /// <summary>
    /// DTO for trend data source
    /// </summary>
    public class TrendDataSourceDto : EntityDto<Guid>
    {
        /// <summary>
        /// Name of the source
        /// </summary>
        public string SourceName { get; set; }

        /// <summary>
        /// URL of the source
        /// </summary>
        public string SourceUrl { get; set; }

        /// <summary>
        /// Type of source (research, news, etc.)
        /// </summary>
        public string SourceType { get; set; }

        /// <summary>
        /// Reliability score of the source (0-1 scale)
        /// </summary>
        public decimal Reliability { get; set; }

        /// <summary>
        /// Date when the data was collected
        /// </summary>
        public DateTime CollectedAt { get; set; }

        /// <summary>
        /// Data quality assessment
        /// </summary>
        public string DataQuality { get; set; }
    }
}
