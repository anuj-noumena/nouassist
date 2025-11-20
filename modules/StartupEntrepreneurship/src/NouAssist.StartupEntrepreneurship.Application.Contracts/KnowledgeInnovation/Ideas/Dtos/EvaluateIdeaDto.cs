using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Ideas.Dtos
{
    public class EvaluateIdeaDto
    {
        [StringLength(2000)]
        public string Comments { get; set; }

        [Required]
        public bool IsApproved { get; set; }

        public List<CreateEvaluationScoreDto> Scores { get; set; }
    }

    public class CreateEvaluationScoreDto
    {
        [Required]
        [StringLength(64)]
        public string CriteriaName { get; set; }

        [Required]
        [Range(1, 10)]
        public int Score { get; set; }

        [StringLength(500)]
        public string Justification { get; set; }
    }
}
