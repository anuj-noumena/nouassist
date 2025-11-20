using System;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.CustomerSegment.Dtos
{
    /// <summary>
    /// DTO for segment characteristics
    /// </summary>
    public class SegmentCharacteristicsDto : EntityDto<Guid>
    {
        /// <summary>
        /// Characteristic name
        /// </summary>
        public string CharacteristicName { get; set; }

        /// <summary>
        /// Characteristic value
        /// </summary>
        public string CharacteristicValue { get; set; }

        /// <summary>
        /// Category of the characteristic
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Importance score (0-1 scale)
        /// </summary>
        public decimal Importance { get; set; }

        /// <summary>
        /// Additional notes
        /// </summary>
        public string Notes { get; set; }
    }
}
