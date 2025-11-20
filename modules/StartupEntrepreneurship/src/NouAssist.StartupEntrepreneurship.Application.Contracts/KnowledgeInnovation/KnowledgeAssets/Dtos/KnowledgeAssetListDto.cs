using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.KnowledgeAssets.Dtos
{
    public class KnowledgeAssetListDto : AuditedEntityDto<Guid>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public KnowledgeAssetType AssetType { get; set; }
        public AuthorInfoDto Author { get; set; }
        public List<string> Tags { get; set; }
    }
}
