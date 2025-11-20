using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.GlossaryTerms.Dtos
{
    /// <summary>
    /// DTO representing a glossary term
    /// </summary>
    public class GlossaryTermDto : FullAuditedEntityDto<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public string Definition { get; set; } = string.Empty;
        public string? Context { get; set; }
        public string? Examples { get; set; }
        public List<Guid> RelatedTermIds { get; set; } = new();
        public string? Synonyms { get; set; }
        public string? Acronyms { get; set; }
        public bool IsApproved { get; set; }
        public Guid? ApprovedBy { get; set; }
        public DateTime? ApprovedAt { get; set; }
    }
}
