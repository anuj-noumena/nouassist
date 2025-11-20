using System.ComponentModel.DataAnnotations;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.GlossaryTerms.Dtos
{
    /// <summary>
    /// DTO for setting synonyms and acronyms
    /// </summary>
    public class SetAlternativeFormsDto
    {
        [StringLength(500)]
        public string? Synonyms { get; set; }

        [StringLength(200)]
        public string? Acronyms { get; set; }
    }
}
