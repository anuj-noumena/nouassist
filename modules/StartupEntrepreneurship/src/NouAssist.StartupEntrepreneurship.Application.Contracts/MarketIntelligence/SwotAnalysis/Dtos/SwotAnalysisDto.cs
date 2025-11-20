using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.SwotAnalysis.Dtos
{
    /// <summary>
    /// DTO for SWOT analysis data
    /// </summary>
    public class SwotAnalysisDto : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// Analysis title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Analysis description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Date when the analysis was created
        /// </summary>
        public DateTime AnalysisDate { get; set; }

        /// <summary>
        /// Date when the analysis was completed
        /// </summary>
        public DateTime? CompletedDate { get; set; }

        /// <summary>
        /// Whether the analysis is completed
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Strategic recommendation
        /// </summary>
        public string StrategicRecommendation { get; set; }

        /// <summary>
        /// Analysis notes
        /// </summary>
        public string AnalysisNotes { get; set; }

        /// <summary>
        /// SWOT items
        /// </summary>
        public List<SwotItemDto> Items { get; set; }
    }
}
