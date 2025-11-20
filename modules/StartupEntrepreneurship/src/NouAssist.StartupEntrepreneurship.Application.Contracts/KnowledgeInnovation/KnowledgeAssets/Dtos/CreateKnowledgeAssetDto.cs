using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Constants;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Enums;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.KnowledgeAssets.Dtos
{
    public class CreateKnowledgeAssetDto
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

        [Required]
        public CreateAuthorInfoDto Author { get; set; }

        [Required]
        [StringLength(KnowledgeInnovationConsts.KnowledgeAsset.ContentMaxLength)]
        public string Content { get; set; }

        [Required]
        [StringLength(100)]
        public string ContentType { get; set; }

        public List<string> Tags { get; set; }
    }

    public class CreateAuthorInfoDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        [StringLength(KnowledgeInnovationConsts.DefaultLengths.Name)]
        public string Name { get; set; }

        [Required]
        [StringLength(KnowledgeInnovationConsts.DefaultLengths.Title)]
        public string Title { get; set; }

        [StringLength(KnowledgeInnovationConsts.DefaultLengths.Url)]
        public string AvatarUrl { get; set; }
    }
}
