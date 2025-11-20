using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.KnowledgeAssets.Dtos
{
    public class GetKnowledgeAssetListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public KnowledgeAssetType? AssetType { get; set; }
        public string Tag { get; set; }
    }
}
