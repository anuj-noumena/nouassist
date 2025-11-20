using System;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Enums;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.AnalyzedRequirements.Dtos
{
    /// <summary>
    /// DTO representing an attribute value
    /// </summary>
    public class AttributeValueDto
    {
        public string StringValue { get; set; } = string.Empty;
        public decimal? NumericValue { get; set; }
        public bool? BooleanValue { get; set; }
        public DateTime? DateTimeValue { get; set; }
        public AttributeType ValueType { get; set; }
    }
}
