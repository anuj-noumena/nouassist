using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Constants;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.DecisionRecords.Dtos
{
    public class MakeDecisionDto
    {
        [Required]
        [StringLength(KnowledgeInnovationConsts.DefaultLengths.Title)]
        public string ChosenAlternativeTitle { get; set; }

        [Required]
        [StringLength(KnowledgeInnovationConsts.DecisionRecord.RationaleMaxLength)]
        public string Reasoning { get; set; }

        public List<string> KeyFactors { get; set; }

        public List<string> RisksIdentified { get; set; }

        public List<string> AssumptionsMade { get; set; }

        public DateTime DecisionDate { get; set; } = DateTime.UtcNow;
    }
}
