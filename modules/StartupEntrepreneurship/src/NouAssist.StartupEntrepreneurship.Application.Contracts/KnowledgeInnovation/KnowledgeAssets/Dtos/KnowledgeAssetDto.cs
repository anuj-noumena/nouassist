using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.KnowledgeAssets.Dtos
{
    public class KnowledgeAssetDto : AuditedEntityDto<Guid>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public KnowledgeAssetType AssetType { get; set; }
        public AuthorInfoDto Author { get; set; }
        public List<string> Tags { get; set; }
        public AssetContentDto CurrentContent { get; set; }
    }

    public class AuthorInfoDto
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string AvatarUrl { get; set; }
    }

    public class AssetContentDto
    {
        public string Content { get; set; }
        public string ContentType { get; set; }
        public int Version { get; set; }
        public string ChangeLog { get; set; }
    }
}
