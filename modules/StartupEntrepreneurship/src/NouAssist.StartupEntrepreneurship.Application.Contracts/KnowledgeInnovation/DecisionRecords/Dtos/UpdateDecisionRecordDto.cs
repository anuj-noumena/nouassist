using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Constants;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.DecisionRecords.Dtos
{
    public class UpdateDecisionRecordDto
    {
        [Required]
        [StringLength(KnowledgeInnovationConsts.DecisionRecord.TitleMaxLength, 
            MinimumLength = KnowledgeInnovationConsts.DecisionRecord.TitleMinLength)]
        public string Title { get; set; }

        public List<string> Tags { get; set; }
    }
}
