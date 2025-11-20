using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.LearningResources.Dtos
{
    public class LearningResourceDto : AuditedEntityDto<Guid>
    {
        public string Title { get; set; }
        public ResourceType ResourceType { get; set; }
        public string ResourceUrl { get; set; }
        public LearningModuleDescriptionDto Description { get; set; }
        public List<string> Tags { get; set; }
        public bool IsPublished { get; set; }
        public bool IsFeatured { get; set; }
        public UserProgressDto UserProgress { get; set; }
    }

    public class LearningModuleDescriptionDto
    {
        public string Overview { get; set; }
        public string LearningObjectives { get; set; }
        public string PrerequisiteKnowledge { get; set; }
        public int EstimatedDurationMinutes { get; set; }
    }

    public class UserProgressDto
    {
        public CompletionStatus Status { get; set; }
        public int ProgressPercentage { get; set; }
        public DateTime LastAccessedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
