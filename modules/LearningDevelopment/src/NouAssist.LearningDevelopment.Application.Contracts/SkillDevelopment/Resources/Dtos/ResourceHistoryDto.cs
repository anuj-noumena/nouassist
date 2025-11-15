using System;
using System.Collections.Generic;
using NouAssist.SkillDevelopment.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.SkillDevelopment.Resources.Dtos
{
    public class ResourceHistoryDto : FullAuditedEntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public string ResourceId { get; set; }
        public string Title { get; set; }
        public ResourceType Type { get; set; }
        public string SourceUrl { get; set; }
        public DateTime AccessedAt { get; set; }
        public int Progress { get; set; }
        public List<Guid> RelatedSkillIds { get; set; }
        public List<string> RelatedSkillTitles { get; set; }
    }

    public class CreateResourceHistoryDto
    {
        public string ResourceId { get; set; }
        public string Title { get; set; }
        public ResourceType Type { get; set; }
        public string SourceUrl { get; set; }
        public List<Guid> RelatedSkillIds { get; set; }
    }

    public class UpdateResourceProgressDto
    {
        public int Progress { get; set; }
    }

    public class GetResourceHistoryListDto : PagedAndSortedResultRequestDto
    {
        public Guid? SkillId { get; set; }
        public ResourceType? Type { get; set; }
        public bool? CompletedOnly { get; set; }
    }
}
