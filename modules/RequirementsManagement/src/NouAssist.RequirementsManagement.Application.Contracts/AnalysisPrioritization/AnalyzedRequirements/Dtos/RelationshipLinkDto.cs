using System;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Enums;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.AnalyzedRequirements.Dtos
{
    /// <summary>
    /// DTO representing a relationship link between requirements
    /// </summary>
    public class RelationshipLinkDto
    {
        public Guid Id { get; set; }
        public Guid SourceRequirementId { get; set; }
        public Guid TargetRequirementId { get; set; }
        public RelationshipType RelationshipType { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedTime { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
