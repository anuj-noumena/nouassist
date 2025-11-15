using System;
using NouAssist.Learning.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.Learning.Topics.Dtos
{
    public class GetTopicListDto : PagedAndSortedResultRequestDto
    {
        public TopicStatus? Status { get; set; }
        public DifficultyLevel? Difficulty { get; set; }
        public string Category { get; set; }
        public bool? IsFolder { get; set; }
        public Guid? ParentId { get; set; }
        public string SearchKeyword { get; set; }
        public int? MinProgress { get; set; }
        public int? MaxProgress { get; set; }
    }
}
