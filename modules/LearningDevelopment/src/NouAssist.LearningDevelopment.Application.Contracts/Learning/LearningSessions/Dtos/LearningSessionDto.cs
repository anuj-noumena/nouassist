using System;
using System.Collections.Generic;
using NouAssist.Learning.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.Learning.LearningSessions.Dtos
{
    public class LearningSessionDto : FullAuditedEntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public Guid TopicId { get; set; }
        public LearningPhase CurrentPhase { get; set; }
        public int ReviewCycle { get; set; }
        public bool Completed { get; set; }
        public bool Paused { get; set; }
        public DateTime? PausedAt { get; set; }
        public DateTime? ResumedAt { get; set; }
        public DateTime LastActivity { get; set; }
        public Dictionary<int, PhaseMilestoneDto> PhaseMilestones { get; set; }
        public Dictionary<int, int> TimeTracking { get; set; }
        public int TotalTimeSpentMs { get; set; }
    }

    public class PhaseMilestoneDto
    {
        public LearningPhase Phase { get; set; }
        public bool Completed { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int? TimeSpentMs { get; set; }
        public List<string> Questions { get; set; }
        public Dictionary<string, string> SectionReflections { get; set; }
        public string CornellSummary { get; set; }
        public BloomAssessmentResultDto BloomAssessment { get; set; }
        public Dictionary<int, CycleProgressDto> SpacedCycles { get; set; }
        public Dictionary<string, object> MasteryValidation { get; set; }
    }

    public class BloomAssessmentResultDto
    {
        public double RememberScore { get; set; }
        public double UnderstandScore { get; set; }
        public double ApplyScore { get; set; }
        public double AnalyzeScore { get; set; }
        public double EvaluateScore { get; set; }
        public double CreateScore { get; set; }
        public double OverallScore { get; set; }
    }

    public class CycleProgressDto
    {
        public int CycleNumber { get; set; }
        public bool Completed { get; set; }
        public DateTime? CompletedAt { get; set; }
        public double? RecallAccuracy { get; set; }
        public double? AverageConfidence { get; set; }
        public int? CompletionRate { get; set; }
        public Dictionary<string, bool> Improvements { get; set; }
        public Dictionary<string, string> Reflection { get; set; }
    }
}
