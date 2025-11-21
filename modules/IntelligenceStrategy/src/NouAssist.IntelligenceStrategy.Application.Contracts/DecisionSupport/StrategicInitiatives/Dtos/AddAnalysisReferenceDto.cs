using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.IntelligenceStrategy.DecisionSupport.Constants;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.StrategicInitiatives.Dtos
{
    /// <summary>
    /// DTO for adding a supporting analysis reference to an initiative
    /// </summary>
    public class AddAnalysisReferenceDto
    {
        /// <summary>
        /// Type of analysis (e.g., "SWOT", "ROI", "RiskAssessment")
        /// </summary>
        [Required]
        [StringLength(50)]
        public string ReferenceType { get; set; }
        
        /// <summary>
        /// Key or ID to locate the reference
        /// </summary>
        [Required]
        [StringLength(DecisionSupportConsts.MaxReferenceKeyLength)]
        public string ReferenceKey { get; set; }
        
        /// <summary>
        /// Description of the analysis and how it supports the initiative
        /// </summary>
        [Required]
        [StringLength(DecisionSupportConsts.MaxDescriptionLength)]
        public string Description { get; set; }
    }
}
