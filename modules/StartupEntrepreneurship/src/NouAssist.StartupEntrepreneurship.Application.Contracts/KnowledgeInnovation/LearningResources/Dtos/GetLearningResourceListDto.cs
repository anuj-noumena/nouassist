using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.LearningResources.Dtos
{
    public class GetLearningResourceListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public ResourceType? ResourceType { get; set; }
        public string Tag { get; set; }
        public bool? OnlyPublished { get; set; }
        public bool? OnlyFeatured { get; set; }
        public CompletionStatus? CompletionStatus { get; set; }
    }
}
