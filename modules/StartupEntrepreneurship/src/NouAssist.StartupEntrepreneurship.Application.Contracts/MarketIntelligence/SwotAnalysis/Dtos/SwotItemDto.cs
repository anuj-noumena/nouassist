using System;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.SwotAnalysis.Dtos
{
    /// <summary>
    /// DTO for SWOT item data
    /// </summary>
    public class SwotItemDto : EntityDto<Guid>
    {
        /// <summary>
        /// SWOT category (Strength, Weakness, Opportunity, Threat)
        /// </summary>
        public SwotCategory Category { get; set; }

        /// <summary>
        /// Item title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Item description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Impact score (0-1 scale)
        /// </summary>
        public decimal Impact { get; set; }

        /// <summary>
        /// Priority level (1-10, lower is higher priority)
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Action items related to this SWOT item
        /// </summary>
        public string ActionItems { get; set; }

        /// <summary>
        /// Tags for the item
        /// </summary>
        public string Tags { get; set; }
    }
}
