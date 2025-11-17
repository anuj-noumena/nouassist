using System;
using System.Collections.Generic;
using NouAssist.Learning.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.LearningDevelopment.Learning.LearningNotes.Dtos
{
    public class LearningNoteDto : FullAuditedEntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public Guid TopicId { get; set; }
        public LearningNoteType Type { get; set; }
        public string Questions { get; set; }
        public string Notes { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public List<string> KeyPoints { get; set; }
        public Dictionary<string, object> Metadata { get; set; }
    }
}
