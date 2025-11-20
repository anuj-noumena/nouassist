using System;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Enums;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.AnalyzedRequirements.Dtos
{
    /// <summary>
    /// DTO representing a requirement attribute instance
    /// </summary>
    public class RequirementAttributeDto
    {
        public Guid Id { get; set; }
        public string AttributeName { get; set; } = string.Empty;
        public AttributeType AttributeType { get; set; }
        public AttributeValueDto AttributeValue { get; set; } = new();
        public DateTime CreatedTime { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
