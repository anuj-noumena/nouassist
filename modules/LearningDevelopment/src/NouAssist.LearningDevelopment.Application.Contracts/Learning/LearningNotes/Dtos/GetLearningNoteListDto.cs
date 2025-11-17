using System;
using NouAssist.Learning.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.LearningDevelopment.Learning.LearningNotes.Dtos
{
    public class GetLearningNoteListDto : PagedAndSortedResultRequestDto
    {
        public Guid? TopicId { get; set; }
        public LearningNoteType? Type { get; set; }
        public string SearchKeyword { get; set; }
    }
}
