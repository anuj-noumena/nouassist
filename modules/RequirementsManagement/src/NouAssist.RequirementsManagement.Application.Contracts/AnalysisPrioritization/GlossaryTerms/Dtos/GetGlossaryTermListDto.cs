using Volo.Abp.Application.Dtos;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.GlossaryTerms.Dtos
{
    /// <summary>
    /// DTO for filtering glossary terms list
    /// </summary>
    public class GetGlossaryTermListDto : PagedAndSortedResultRequestDto
    {
        public string? Filter { get; set; }
        public bool? ApprovedOnly { get; set; }
        public bool? HasSynonyms { get; set; }
        public bool? HasAcronyms { get; set; }
    }
}
