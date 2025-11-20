using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Constants;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.DecisionRecords.Dtos
{
    public class AddAlternativeDto
    {
        [Required]
        [StringLength(KnowledgeInnovationConsts.DefaultLengths.Title)]
        public string Title { get; set; }

        [Required]
        [StringLength(KnowledgeInnovationConsts.DecisionRecord.ContextMaxLength)]
        public string Description { get; set; }

        [StringLength(2000)]
        public string ProsCons { get; set; }
    }
}
