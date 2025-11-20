using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.LearningResources.Dtos
{
    public class LearningResourceListDto : AuditedEntityDto<Guid>
    {
        public string Title { get; set; }
        public ResourceType ResourceType { get; set; }
        public int EstimatedDurationMinutes { get; set; }
        public List<string> Tags { get; set; }
        public bool IsPublished { get; set; }
        public bool IsFeatured { get; set; }
        public CompletionStatus? UserCompletionStatus { get; set; }
    }
}
