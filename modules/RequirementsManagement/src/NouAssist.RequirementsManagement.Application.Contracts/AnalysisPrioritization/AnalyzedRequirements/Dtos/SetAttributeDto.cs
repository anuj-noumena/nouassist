using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Enums;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Constants;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.AnalyzedRequirements.Dtos
{
    /// <summary>
    /// DTO for setting an attribute on an analyzed requirement
    /// </summary>
    public class SetAttributeDto
    {
        [Required]
        [StringLength(AnalysisConsts.MaxAttributeNameLength)]
        public string AttributeName { get; set; } = string.Empty;

        [Required]
        public AttributeType AttributeType { get; set; }

        [Required]
        public AttributeValueDto AttributeValue { get; set; } = new();
    }
}
