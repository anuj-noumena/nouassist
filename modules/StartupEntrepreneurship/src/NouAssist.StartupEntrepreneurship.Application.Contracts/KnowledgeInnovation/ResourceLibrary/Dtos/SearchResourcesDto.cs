using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Enums;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.ResourceLibrary.Dtos
{
    public class SearchResourcesDto
    {
        public string SearchText { get; set; }
        public KnowledgeAssetType? AssetType { get; set; }
        public ResourceType? ResourceType { get; set; }
        public List<string> Tags { get; set; }
        public int MaxResults { get; set; } = 10;
    }
}
