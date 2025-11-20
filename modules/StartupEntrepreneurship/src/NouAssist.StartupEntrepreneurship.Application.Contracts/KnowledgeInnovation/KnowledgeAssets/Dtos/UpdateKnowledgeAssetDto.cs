using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Constants;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Enums;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.KnowledgeAssets.Dtos
{
    public class UpdateKnowledgeAssetDto
    {
        [Required]
        [StringLength(KnowledgeInnovationConsts.KnowledgeAsset.TitleMaxLength, 
            MinimumLength = KnowledgeInnovationConsts.KnowledgeAsset.TitleMinLength)]
        public string Title { get; set; }

        [Required]
        [StringLength(KnowledgeInnovationConsts.KnowledgeAsset.DescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        public KnowledgeAssetType AssetType { get; set; }

        public List<string> Tags { get; set; }
    }
}
