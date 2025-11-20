using System;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Constants;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.DecisionRecords.Dtos
{
    public class ImplementDecisionDto
    {
        [Required]
        [StringLength(KnowledgeInnovationConsts.DecisionRecord.OutcomeMaxLength)]
        public string Description { get; set; }

        public DateTime ImplementationDate { get; set; } = DateTime.UtcNow;
    }
}
