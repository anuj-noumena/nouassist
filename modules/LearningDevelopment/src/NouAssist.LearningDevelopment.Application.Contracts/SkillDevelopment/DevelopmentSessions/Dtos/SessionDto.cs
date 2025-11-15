using System;
using System.Collections.Generic;
using NouAssist.SkillDevelopment.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.SkillDevelopment.DevelopmentSessions.Dtos
{
    public class SkillDevelopmentSessionDto : FullAuditedEntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public Guid SkillId { get; set; }
        public string SkillTitle { get; set; }
        public DevelopmentStep CurrentStep { get; set; }
        public int ReviewCycle { get; set; }
        public bool Completed { get; set; }
        public bool Paused { get; set; }
        public DateTime? PausedAt { get; set; }
        public DateTime? ResumedAt { get; set; }
        public DateTime LastActivity { get; set; }
        public int? OverallProficiencyGain { get; set; }
        public int TotalPracticeMinutes { get; set; }
        public Dictionary<DevelopmentStep, SkillMilestoneDto> Milestones { get; set; }
    }

    public class SkillMilestoneDto
    {
        public DevelopmentStep Step { get; set; }
        public bool Completed { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int TotalMinutes { get; set; }
        public DateTime UpdatedAt { get; set; }
        public AssessmentResultDto InitialAssessment { get; set; }
        public List<PracticePlanItemDto> PracticePlan { get; set; }
        public List<PracticeLogDto> PracticeLogs { get; set; }
        public Dictionary<string, int> ReviewFeedback { get; set; }
        public List<ApplicationExampleDto> Applications { get; set; }
        public Dictionary<string, object> MasteryValidation { get; set; }
    }

    public class AssessmentResultDto
    {
        public Dictionary<string, int> Scores { get; set; }
        public List<string> StrengthAreas { get; set; }
        public List<string> ImprovementAreas { get; set; }
        public string OverallNotes { get; set; }
        public int AverageScore { get; set; }
    }

    public class PracticePlanItemDto
    {
        public string Activity { get; set; }
        public int EstimatedMinutes { get; set; }
        public string Objective { get; set; }
        public List<string> Resources { get; set; }
    }

    public class PracticeLogDto
    {
        public Guid PracticeLogId { get; set; }
        public DateTime Date { get; set; }
        public int DurationMinutes { get; set; }
        public string FocusArea { get; set; }
        public int? SelfRating { get; set; }
        public List<string> ChallengesFaced { get; set; }
        public string Notes { get; set; }
    }

    public class ApplicationExampleDto
    {
        public Guid ExampleId { get; set; }
        public string Description { get; set; }
        public List<string> Outcomes { get; set; }
        public int? ProficiencyImpact { get; set; }
        public DateTime CompletedAt { get; set; }
    }

    public class StartDevelopmentSessionDto
    {
        public Guid SkillId { get; set; }
    }

    public class RecordAssessmentDto
    {
        public Dictionary<string, int> Scores { get; set; }
        public List<string> StrengthAreas { get; set; }
        public List<string> ImprovementAreas { get; set; }
        public string OverallNotes { get; set; }
    }

    public class AddPracticePlanDto
    {
        public List<PracticePlanItemDto> PlanItems { get; set; }
    }

    public class AddPracticeLogDto
    {
        public DateTime Date { get; set; }
        public int DurationMinutes { get; set; }
        public string FocusArea { get; set; }
        public int? SelfRating { get; set; }
        public List<string> ChallengesFaced { get; set; }
        public string Notes { get; set; }
    }

    public class AddReviewFeedbackDto
    {
        public Dictionary<string, int> Feedback { get; set; }
    }

    public class AddApplicationExampleDto
    {
        public string Description { get; set; }
        public List<string> Outcomes { get; set; }
        public int? ProficiencyImpact { get; set; }
        public DateTime CompletedAt { get; set; }
    }

    public class RecordMasteryValidationDto
    {
        public Dictionary<string, object> ValidationData { get; set; }
    }

    public class GetSessionListDto : PagedAndSortedResultRequestDto
    {
        public Guid? SkillId { get; set; }
        public bool? Completed { get; set; }
        public bool? Paused { get; set; }
        public DevelopmentStep? CurrentStep { get; set; }
    }
}
