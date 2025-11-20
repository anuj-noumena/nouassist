using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Constants;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.KnowledgeAssets.Dtos
{
    public class UpdateContentDto
    {
        [Required]
        [StringLength(KnowledgeInnovationConsts.KnowledgeAsset.ContentMaxLength)]
        public string Content { get; set; }

        [StringLength(1000)]
        public string ChangeLog { get; set; }
    }
}
