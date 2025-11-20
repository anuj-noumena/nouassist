using System;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Competitor.Dtos
{
    /// <summary>
    /// DTO for competitor update log entries
    /// </summary>
    public class CompetitorUpdateLogDto : EntityDto<Guid>
    {
        /// <summary>
        /// The type of update
        /// </summary>
        public UpdateType Type { get; set; }

        /// <summary>
        /// Update title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Update description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Information source
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Date of the update
        /// </summary>
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// Impact level of the update
        /// </summary>
        public ImpactLevel ImpactLevel { get; set; }

        /// <summary>
        /// Tags for the update
        /// </summary>
        public string Tags { get; set; }
    }
}
