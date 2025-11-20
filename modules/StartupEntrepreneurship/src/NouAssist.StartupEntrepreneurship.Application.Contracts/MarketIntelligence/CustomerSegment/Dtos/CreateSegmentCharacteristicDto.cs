using System.ComponentModel.DataAnnotations;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.CustomerSegment.Dtos
{
    /// <summary>
    /// DTO for creating a segment characteristic
    /// </summary>
    public class CreateSegmentCharacteristicDto
    {
        /// <summary>
        /// Characteristic name
        /// </summary>
        [Required]
        [StringLength(MarketIntelligenceConsts.CustomerSegment.MaxCharacteristicNameLength)]
        public string CharacteristicName { get; set; }

        /// <summary>
        /// Characteristic value
        /// </summary>
        [Required]
        [StringLength(MarketIntelligenceConsts.CustomerSegment.MaxCharacteristicValueLength)]
        public string CharacteristicValue { get; set; }

        /// <summary>
        /// Category of the characteristic
        /// </summary>
        [StringLength(100)]
        public string Category { get; set; }

        /// <summary>
        /// Importance score (0-1 scale)
        /// </summary>
        [Range(0.0, 1.0, ErrorMessage = "Importance must be between 0 and 1")]
        public decimal Importance { get; set; } = 0.5m;

        /// <summary>
        /// Additional notes
        /// </summary>
        [StringLength(MarketIntelligenceConsts.Validation.MaxNotesLength)]
        public string Notes { get; set; }
    }
}
