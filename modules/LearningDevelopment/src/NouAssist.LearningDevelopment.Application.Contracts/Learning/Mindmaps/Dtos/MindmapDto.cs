using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace NouAssist.LearningDevelopment.Learning.Mindmaps.Dtos
{
    public class MindmapDto : FullAuditedEntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public Guid TopicId { get; set; }
        public string Url { get; set; }
        public Dictionary<string, object> Data { get; set; }
    }

    public class CreateMindmapDto
    {
        public Guid TopicId { get; set; }
        public Dictionary<string, object> Data { get; set; }
        public string Url { get; set; }
    }

    public class UpdateMindmapDto
    {
        public Dictionary<string, object> Data { get; set; }
        public string Url { get; set; }
    }
}
