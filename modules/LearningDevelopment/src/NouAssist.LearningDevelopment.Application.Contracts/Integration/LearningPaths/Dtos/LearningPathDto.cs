using System;
using System.Collections.Generic;
using NouAssist.Learning.Enums;
using NouAssist.LearningDevelopment.Integration.Enums;
using NouAssist.SkillDevelopment.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.LearningDevelopment.Integration.LearningPaths.Dtos
{
    public class LearningPathDto : FullAuditedEntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public LearningPathStatus Status { get; set; }
        public LearningPathPhase CurrentPhase { get; set; }

        // Theory component
        public Guid TopicId { get; set; }
        public string TopicTitle { get; set; }
        public Guid? LearningSessionId { get; set; }
        public LearningPhase? CurrentLearningPhase { get; set; }
        public bool TheoryCompleted { get; set; }
        public DateTime? TheoryCompletedAt { get; set; }

        // Practice component
        public Guid SkillId { get; set; }
        public string SkillTitle { get; set; }
        public Guid? SkillDevelopmentSessionId { get; set; }
        public DevelopmentStep? CurrentDevelopmentStep { get; set; }
        public bool PracticeCompleted { get; set; }
        public DateTime? PracticeCompletedAt { get; set; }

        // Integration metrics
        public int TheoryProgressPercentage { get; set; }
        public int PracticeProgressPercentage { get; set; }
        public int OverallProgressPercentage { get; set; }
        public int IntegratedMasteryScore { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime LastActivity { get; set; }

        public List<string> Milestones { get; set; }
    }
}
