using System;
using System.Collections.Generic;
using NouAssist.Learning.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.Learning.Topics.Dtos
{
    public class TopicDto : FullAuditedEntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid? ParentId { get; set; }
        public bool IsFolder { get; set; }
        public string Category { get; set; }
        public DifficultyLevel Difficulty { get; set; }
        public int EstimatedHours { get; set; }
        public string LearningGoals { get; set; }
        public int Progress { get; set; }
        public TopicStatus Status { get; set; }
        public Guid? LearningSessionId { get; set; }
        public List<string> Tags { get; set; }
        public List<TopicDto> Children { get; set; }
    }

    public class TopicSummaryDto : EntityDto<Guid>
    {
        public string Title { get; set; }
        public bool IsFolder { get; set; }
        public TopicStatus Status { get; set; }
        public int Progress { get; set; }
        public DifficultyLevel Difficulty { get; set; }
    }

    public class TopicMetricsDto
    {
        public int TotalTimeSpentMs { get; set; }
        public int EstimatedTimeRemainingMs { get; set; }
        public double CompletionPercentage { get; set; }
        public int ActiveChildCount { get; set; }
        public int CompletedChildCount { get; set; }
        public double AverageProgressOfChildren { get; set; }
    }
}
