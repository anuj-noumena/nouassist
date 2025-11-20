using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Constants;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.DecisionRecords.Dtos
{
    public class UpdateDecisionContextDto
    {
        [Required]
        [StringLength(KnowledgeInnovationConsts.DecisionRecord.ContextMaxLength)]
        public string Background { get; set; }

        [Required]
        [StringLength(KnowledgeInnovationConsts.DecisionRecord.ContextMaxLength)]
        public string ProblemStatement { get; set; }

        public List<string> Stakeholders { get; set; }

        public List<string> Constraints { get; set; }
    }
}
