using System.ComponentModel.DataAnnotations;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Constants;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.GlossaryTerms.Dtos
{
    /// <summary>
    /// DTO for updating a glossary term
    /// </summary>
    public class UpdateGlossaryTermDto
    {
        [Required]
        [StringLength(AnalysisConsts.MaxGlossaryTermDefinitionLength)]
        public string Definition { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Context { get; set; }

        [StringLength(1000)]
        public string? Examples { get; set; }
    }
}
