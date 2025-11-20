using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Constants;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.DecisionRecords.Dtos
{
    public class EvaluateOutcomeDto
    {
        [Required]
        [StringLength(KnowledgeInnovationConsts.DecisionRecord.OutcomeMaxLength)]
        public string Description { get; set; }

        [StringLength(KnowledgeInnovationConsts.DecisionRecord.OutcomeMaxLength)]
        public string LessonsLearned { get; set; }

        public List<CreateOutcomeImpactDto> Impacts { get; set; }
    }

    public class CreateOutcomeImpactDto
    {
        [Required]
        [StringLength(KnowledgeInnovationConsts.DefaultLengths.Name)]
        public string Area { get; set; }

        [Required]
        [StringLength(KnowledgeInnovationConsts.DefaultLengths.ShortDescription)]
        public string Description { get; set; }

        [Required]
        [Range(1, 10)]
        public int Magnitude { get; set; }
    }
}
