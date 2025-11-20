using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Ideas.Dtos
{
    public class GetIdeaListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public IdeaStatus? Status { get; set; }
        public string Tag { get; set; }
        public bool? HighPriorityOnly { get; set; }
        public bool? CreatedByCurrentUser { get; set; }
    }
}
