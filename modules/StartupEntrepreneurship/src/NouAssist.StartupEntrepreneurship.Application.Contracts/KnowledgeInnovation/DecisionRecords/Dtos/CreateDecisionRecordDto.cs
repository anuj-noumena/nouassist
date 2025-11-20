using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Constants;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.DecisionRecords.Dtos
{
    public class CreateDecisionRecordDto
    {
        [Required]
        [StringLength(KnowledgeInnovationConsts.DecisionRecord.TitleMaxLength, 
            MinimumLength = KnowledgeInnovationConsts.DecisionRecord.TitleMinLength)]
        public string Title { get; set; }

        [Required]
        public CreateDecisionContextDto Context { get; set; }

        public List<string> Tags { get; set; }
    }

    public class CreateDecisionContextDto
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
